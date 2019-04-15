using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Calculator.ViewModel
{
    public enum Operation { Add, Sub, Mult, Div, Neg, Equals, Last = Equals };

    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string displayString;
        private readonly ReadOnlyCollection<ICommand> numCommands;
        private readonly ReadOnlyDictionary<Operation, ICommand> opCommands;
        private Operation current = Operation.Add;
        private readonly Model.Model model;

        public ViewModel()
        {
            model = new Model.Model();
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
            // TODO - implement number entry logic here
            Display += num;
        }
        private void OnOperationCommand(Operation op)
        {
            // TODO - Implement
            switch (op)
            {
                case Operation.Add:
                    Display = "Add";
                    break;
                case Operation.Sub:
                    Display = "Subtract";
                    break;
                case Operation.Mult:
                    Display = "Multiply";
                    break;
                case Operation.Div:
                    Display = "Divide";
                    break;
                case Operation.Neg:
                    Display = "Negate";
                    break;
                case Operation.Equals:
                    Display = "Equals";
                    break;
            }
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
