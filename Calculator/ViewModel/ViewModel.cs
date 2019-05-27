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
            ClearCommand = new RelayCommand(OnClear);
            ClearEntryCommand = new RelayCommand(OnClearEntry);
            NegateCommand = new RelayCommand(OnNegate);
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
        public int DisplaySize { private get; set; }
        public ReadOnlyCollection<ICommand> NumCommands { get; }
        public ReadOnlyDictionary<Operation, ICommand> OpCommands { get; }
        public ICommand EvalCommand { get; }
        public ICommand DecCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand ClearEntryCommand { get; }
        public ICommand NegateCommand { get; }
        private bool DisplayFull
        {
            get
            {
                int length = Display.Length;
                if (length != 0 && Display[0] != '-')
                    length++;

                return length >= DisplaySize;
            }
        }

        private void OnNumberCommand(int num)
        {
            state.OnNumber(num);
            if (state.ClearAccumulator)
                model.ClearAccumulator();
            if (state.OverwriteDisplay)
                Display = num.ToString();
            else if (state.ReplaceZero)
                Display = Display.Remove(Display.Length - 1, 1) + num;
            else if (state.AppendNumber && !DisplayFull)
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
            if (state.ClearAccumulator)
                model.ClearAccumulator();
            if (state.OverwriteDisplay)
                Display = "0.";
            else if (state.AddZeroDecimal)
                Display += "0.";
            else if (state.AddDecimal && !DisplayFull)
                Display += ".";
            state.OnDecimalPost();
        }
        private void OnClear()
        {
            state.OnClear();
            Display = "";
        }
        private void OnClearEntry()
        {
            state.OnClearEntry();
            Display = "";
        }
        private void OnNegate()
        {
            state.OnNegative();
            if (state.ClearAccumulator)
                model.ClearAccumulator();
            if (state.OverwriteDisplay)
                Display = "-";
            else if (state.RemoveNegative)
                Display = Display.Substring(1);
            else
                Display = '-' + Display;
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
            int precision = DisplaySize;
            string formatted;

            do
            {
                formatted = model.Accumulator.ToString("G" + precision);
                precision--;
            } while (formatted.Length > DisplaySize);

            Display = formatted;
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
