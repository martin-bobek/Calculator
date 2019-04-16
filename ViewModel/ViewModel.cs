using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Calculator.ViewModel
{
    public enum Operation { Add, Sub, Mult, Div, Equals, Last = Equals };

    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string displayString;
        private readonly ReadOnlyCollection<ICommand> numCommands;
        private readonly ReadOnlyDictionary<Operation, ICommand> opCommands;
        private readonly Model.Model model;
        private Operation current;
        private readonly KeyState state;

        public ViewModel()
        {
            model = new Model.Model();
            state = new KeyState();
            CreateCommands(out numCommands, out opCommands);
        }

        public string Display
        {
            // TODO - Prevent display from overfilling
            get { return displayString; }
            private set
            {
                if (displayString == value)
                    return;

                displayString = value;
                OnPropertyChanged("Display");
            }
        }
        public ReadOnlyCollection<ICommand> NumCommands
        {
            get { return numCommands; }
        }
        public ReadOnlyDictionary<Operation, ICommand> OpCommands
        {
            get { return opCommands; }
        }

        private void OnNumberCommand(int num)
        {
            if (state.CanOverwrite)
            {
                Display = num.ToString();
                state.OnOverwrite();
            }
            else
                Display += num;
        }
        private void OnOperationCommand(Operation op)
        {
            if (state.CanPerformOperation)
            {
                PerformCurrentOperation();
                state.OnOperationPerformed();
            }
            if (state.CanStoreOperation)
                current = op;
        }
        private void PerformCurrentOperation()
        {
            double value = double.Parse(displayString);
            model.Operand = value;
            switch (current)
            {
                case Operation.Add:
                    model.Add();
                    break;
                case Operation.Sub:
                    model.Subtract();
                    break;
                case Operation.Mult:
                    model.Multiply();
                    break;
                case Operation.Div:
                    model.Divide();
                    break;
                case Operation.Equals:
                    model.ClearAccumulator();
                    model.Add();
                    break;
            }
            Display = model.Accumulator.ToString();
        }
        private void CreateCommands(out ReadOnlyCollection<ICommand> numCommands,
                                    out ReadOnlyDictionary<Operation, ICommand> opCommands)
        {
            numCommands = new ReadOnlyCollection<ICommand>(CreateNumCommands());
            opCommands = new ReadOnlyDictionary<Operation, ICommand>(CreateOpCommands());
        }
        private List<ICommand> CreateNumCommands()
        {
            var commands = new List<ICommand>();
            for (int num = 0; num <= 9; num++)
            {
                int captured = num;
                commands.Add(new RelayCommand(() => OnNumberCommand(captured)));
            }
            return commands;
        }
        private Dictionary<Operation, ICommand> CreateOpCommands()
        {
            var commands = new Dictionary<Operation, ICommand>();
            for (Operation op = 0; op <= Operation.Last; op++)
            {
                Operation captured = op;
                commands.Add(captured, new RelayCommand(() => OnOperationCommand(captured)));
            }
            return commands;
        }
        private void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        [Conditional("DEBUG")]
        private void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
                Debug.Fail("Property does not exist: " + propertyName);
        }
    }
}
