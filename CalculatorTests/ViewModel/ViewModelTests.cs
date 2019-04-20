using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Calculator.ViewModel.Tests
{
    [TestClass]
    public class ViewModelTests
    {
        [TestMethod]
        public void BasicNumberEntry()
        {
            var vm = CreateViewModel();
            EnterNumber(vm, "9876543210");
        }
        [TestMethod]
        public void OperationChain()
        {
            var vm = CreateViewModel();

            EnterNumber(vm, "12");
            EnterOp(vm, Operation.Add, "12");
            EnterNumber(vm, "32");
            EnterOp(vm, Operation.Div, "44");
            EnterNumber(vm, "11");
            EnterOp(vm, Operation.Mult, "4");
            EnterNumber(vm, "3");
            EnterOp(vm, Operation.Sub, "12");
            EnterNumber(vm, "31");
            KeyPress(vm, "=", "-19");
        }
        [TestMethod]
        public void RepeatedOperations()
        {
            var vm = CreateViewModel();

            EnterNumber(vm, "172");
            EnterOp(vm, Operation.Add, "172");
            EnterNumber(vm, "3");

            KeyPress(vm, "=", "175");
            KeyPress(vm, "=", "178");
            KeyPress(vm, "=", "181");

            EnterOp(vm, Operation.Sub, "181");
            EnterNumber(vm, "100");

            KeyPress(vm, "=", "81");
            KeyPress(vm, "=", "-19");
            KeyPress(vm, "=", "-119");
        }
        [TestMethod]
        public void ChainingBreak()
        {
            var vm = CreateViewModel();

            EnterNumber(vm, "1");
            EnterOp(vm, Operation.Add, "1");
            EnterNumber(vm, "2");

            KeyPress(vm, "=", "3");
            EnterOp(vm, Operation.Sub, "3");
            KeyPress(vm, "=", "3");

            EnterNumber(vm, "7");
            EnterOp(vm, Operation.Mult, "7");
            EnterNumber(vm, "3");
            KeyPress(vm, "=", "21");
        }
        [TestMethod]
        public void ChainingBreakContinuation()
        {
            var vm = CreateViewModel();

            EnterNumber(vm, "1");
            EnterOp(vm, Operation.Add, "1");
            EnterNumber(vm, "2");

            KeyPress(vm, "=", "3");
            EnterOp(vm, Operation.Sub, "3");
            KeyPress(vm, "=", "3");

            EnterOp(vm, Operation.Mult, "3");
            EnterNumber(vm, "7");
            KeyPress(vm, "=", "21");
        }

        private ViewModel CreateViewModel()
        {
            ViewModel vm = new ViewModel();
            Assert.AreEqual("", vm.Display);
            return vm;
        }
        private void EnterNumber(ViewModel vm, string input)
        {
            foreach (char keypress in input)
            {
                int num = keypress - '0';
                Assert.IsTrue(vm.NumCommands[num].CanExecute(null));
                vm.NumCommands[num].Execute(null);
            }
            Assert.AreEqual(input, vm.Display);
        }
        private void EnterOp(ViewModel vm, Operation op, string outcome = null)
        {
            Assert.IsTrue(vm.OpCommands[op].CanExecute(null));
            vm.OpCommands[op].Execute(null);
            if (outcome != null)
                Assert.AreEqual(outcome, vm.Display);
        }
        private void KeyPress(ViewModel vm, string key, string outcome = null)
        {
            switch (key)
            {
                case "=":
                    Assert.IsTrue(vm.EvaluateCommand.CanExecute(null));
                    vm.EvaluateCommand.Execute(null);
                    break;
                default:
                    throw new ArgumentException("Operation not supported: " + key, "key");
            }

            if (outcome != null)
                Assert.AreEqual(outcome, vm.Display);
        }
    }
}
