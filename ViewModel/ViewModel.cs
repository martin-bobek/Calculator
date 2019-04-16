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
        private readonly ICommand negCommand;
        private readonly Model.Model model;
        private Operation current;
        private bool overwriteDisplay = true;
        private bool cleared = true;

        public ViewModel()
        {
            model = new Model.Model();
            CreateCommands(out numCommands, 
                           out opCommands, 
                           out negCommand);
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
        public ICommand NegateCommand
        {
            get { return negCommand; }
        }

        private void OnNumberCommand(int num)
        {
            if (overwriteDisplay)
            {
                Display = num.ToString();
                overwriteDisplay = false;
            }
            else
                Display += num;
            cleared = false;
        }
        private void OnOperationCommand(Operation op)
        {
            if (!cleared && !overwriteDisplay)
                PerformCurrentOperation();
            if (!cleared)
                current = op;
            overwriteDisplay = true;
        }
        private void OnNegateCommand()
        {
            if (overwriteDisplay)
            {
                if (cleared || current != Operation.Equals)
                {
                    Display = "-";
                    overwriteDisplay = false;
                }
                else if (overwriteDisplay)
                {
                    model.Multiply(-1);
                    Display = model.Accumulator.ToString();
                    current = Operation.Equals;
                }
            }
            else
            {
                if (Display == "")
                    Display = "-";
                else if (Display[0] == '-')
                    Display = Display.Substring(1);
                else
                    Display = "-" + Display;
            }
        }
        private void PerformCurrentOperation()
        {
            double value = double.Parse(displayString);
            switch (current)
            {
                case Operation.Add:
                    model.Add(value);
                    break;
                case Operation.Sub:
                    model.Subtract(value);
                    break;
                case Operation.Mult:
                    model.Multiply(value);
                    break;
                case Operation.Div:
                    model.Divide(value);
                    break;
                case Operation.Equals:
                    model.ClearAccumulator();
                    model.Add(value);
                    break;
            }
            Display = model.Accumulator.ToString();
        }
        private void CreateCommands(out ReadOnlyCollection<ICommand> numCommands,
                                    out ReadOnlyDictionary<Operation, ICommand> opCommands,
                                    out ICommand negCommand)
        {
            numCommands = new ReadOnlyCollection<ICommand>(CreateNumCommands());
            opCommands = new ReadOnlyDictionary<Operation, ICommand>(CreateOpCommands());
            negCommand = new RelayCommand(OnNegateCommand);
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
