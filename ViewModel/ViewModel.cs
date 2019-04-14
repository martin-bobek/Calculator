using System;
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
        private string displayString;
        private readonly ReadOnlyCollection<ICommand> numCommands;
        private readonly Model.Model model;

        public ViewModel(Model.Model model)
        {
            this.model = model ?? throw new ArgumentNullException("model");
            CreateCommands(out numCommands);
        }

        public string Display
        {
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

        private void OnNumberCommand(int num)
        {
            // TODO - implement number entry logic here
            Display += num;
        }
        private void CreateCommands(out ReadOnlyCollection<ICommand> numCommands)
        {
            numCommands = new ReadOnlyCollection<ICommand>(CreateNumCommands());
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
