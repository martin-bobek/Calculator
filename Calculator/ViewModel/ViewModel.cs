using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Calculator.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Model.Model model;
        private readonly KeyState state;
        private string displayString = "";

        public ViewModel()
        {
            model = new Model.Model();
            state = new KeyState();
            NumCommands = new ReadOnlyCollection<ICommand>(CreateNumCommands());
            OpCommands = new ReadOnlyDictionary<Operation, ICommand>(CreateOpCommands());
            EvalCommand = new RelayCommand(OnEvaluate);
            DecCommand = new RelayCommand(OnDecimal);
            ClearEntryCommand = new RelayCommand(OnClearEntry);
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
        public ReadOnlyCollection<ICommand> NumCommands { get; }
        public ReadOnlyDictionary<Operation, ICommand> OpCommands { get; }
        public ICommand EvalCommand { get; }
        public ICommand DecCommand { get; }
        public ICommand ClearEntryCommand { get; }

        private void OnNumberCommand(int num)
        {
            state.OnNumber();
            if (state.ClearAccumulator)
                model.ClearAccumulator();
            if (state.OverwriteDisplay)
                Display = num.ToString();
            else
                Display += num;
        }
        private void OnOperationCommand(Operation op)
        {
            state.OnOperation();
            if (state.StoreOperand)
                ReadOperand();
            if (state.PerformOperation)
                PerformCurrentOperation();
            if (state.UpdateDisplay)
                UpdateDisplay();
            if (state.StoreOperation)
                state.CurrentOperation = op;
        }
        private void OnEvaluate()
        {
            state.OnEvaluate();
            if (state.StoreOperand)
                ReadOperand();
            if (state.PerformOperation)
                PerformCurrentOperation();
            if (state.UpdateDisplay)
                UpdateDisplay();
        }
        private void OnDecimal()
        {
            state.OnDecimal();          // TODO - Refactor to put at the head of handler.
            if (state.ClearAccumulator)
                model.ClearAccumulator();
            if (state.OverwriteDisplay)
                Display = "0.";
            else if (state.AddDecimal)
                Display += ".";
        }
        private void OnClearEntry()
        {
            state.OnClearEntry();
            Display = "";
        }
        private void PerformCurrentOperation()
        {
            switch (state.CurrentOperation)
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
            }
        }
        private void UpdateDisplay()
        {
            Display = model.Accumulator.ToString();
        }
        private void ReadOperand()
        {
            model.Operand = double.Parse(Display);
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
