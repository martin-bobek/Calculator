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
            KeyPress(vm, "=", "3");

            EnterOp(vm, Operation.Mult, "3");
            EnterNumber(vm, "7");
            KeyPress(vm, "=", "21");
        }
        [TestMethod]
        public void DecimalNumberEntry()
        {
            var vm = CreateViewModel();

            EnterNumber(vm, "120.302..12", "120.30212");
            KeyPress(vm, "=", "120.30212");
            EnterNumber(vm, ".102.2.3...312.", "0.10223312");
            KeyPress(vm, "=", "0.10223312");
            EnterNumber(vm, "1231.", "1231.");
            KeyPress(vm, "=", "1231");
            EnterNumber(vm, "321.", "321.");
            EnterOp(vm, Operation.Add, "321.");
            KeyPress(vm, "=", "321");
        }
        [TestMethod]
        public void NoChainOnImport()
        {
            var vm = CreateViewModel();

            EnterNumber(vm, "1");
            KeyPress(vm, "=", "1");
            KeyPress(vm, "=", "1");
            KeyPress(vm, "=", "1");
        }
        [TestMethod]
        public void ClearEntry()
        {
            var vm = CreateViewModel();

            KeyPress(vm, "CE", "");
            EnterNumber(vm, "1.5.", "1.5");
            KeyPress(vm, "CE", "");
            KeyPress(vm, "=", "");
            EnterNumber(vm, "3.2..", "3.2");

            EnterOp(vm, Operation.Div, "3.2");
            KeyPress(vm, "CE", "");
            EnterNumber(vm, ".4.0.", "0.40");
            EnterOp(vm, Operation.Mult, "8");
            KeyPress(vm, "CE", "");
            KeyPress(vm, "=", "");
            KeyPress(vm, "=", "");

            EnterOp(vm, Operation.Div, "");
            KeyPress(vm, "CE", "");
            EnterNumber(vm, "...40", "0.40");
            KeyPress(vm, "=", "20");
            KeyPress(vm, "CE", "");
            KeyPress(vm, "=", "50");
            KeyPress(vm, "=", "125");

            EnterOp(vm, Operation.Add, "125");
            KeyPress(vm, "=", "125");
            KeyPress(vm, "CE", "");
            KeyPress(vm, "=", "");
            EnterOp(vm, Operation.Sub, "");
            EnterNumber(vm, "190.");
            KeyPress(vm, "=", "-65");

            EnterNumber(vm, "4");
            EnterOp(vm, Operation.Mult, "4");
            KeyPress(vm, "CE", "");
            EnterOp(vm, Operation.Div, "");
            EnterNumber(vm, ".5.0", "0.50");
            KeyPress(vm, "=", "8");
            KeyPress(vm, "=", "16");
        }
        [TestMethod]
        public void ChainingBreakImport()
        {
            var vm = CreateViewModel();

            EnterNumber(vm, "12.");
            EnterOp(vm, Operation.Div, "12.");
            KeyPress(vm, "=", "12");
            EnterNumber(vm, ".125", "0.125");
            KeyPress(vm, "=", "0.125");
            KeyPress(vm, "=", "0.125");
            EnterOp(vm, Operation.Mult, "0.125");
            KeyPress(vm, "=", "0.125");
            EnterNumber(vm, "3");
            EnterOp(vm, Operation.Sub, "3");
            EnterOp(vm, Operation.Sub, "3");
            KeyPress(vm, "=", "3");
            KeyPress(vm, "=", "3");
        }
        [TestMethod]
        public void EvaluateOnReset()
        {
            var vm = CreateViewModel();

            KeyPress(vm, "=", "");
            KeyPress(vm, "=", "");
            KeyPress(vm, "=", "");
            KeyPress(vm, "C", "");

            EnterOp(vm, Operation.Add, "");
            EnterOp(vm, Operation.Add, "");
            EnterOp(vm, Operation.Add, "");
            KeyPress(vm, "C", "");

            EnterOp(vm, Operation.Div, "");
            EnterOp(vm, Operation.Div, "");
            EnterOp(vm, Operation.Div, "");
            KeyPress(vm, "C", "");
        }
        [TestMethod]
        public void NumClearEntryOpDecimal()
        {
            var vm = CreateViewModel();

            EnterNumber(vm, "1");
            KeyPress(vm, "CE", "");
            EnterOp(vm, Operation.Mult, "");

            EnterNumber(vm, ".24.2.43..", "0.24243");
            EnterOp(vm, Operation.Sub, "0.24243");
            EnterNumber(vm, "1.23.", "1.23");
            KeyPress(vm, "=", "-0.98757");
        }
        [TestMethod]
        public void Clear()
        {
            var vm = CreateViewModel();

            EnterNumber(vm, "0.125");
            KeyPress(vm, "C", "");
            KeyPress(vm, "=", "");
            KeyPress(vm, "=", "");

            EnterNumber(vm, "0.43..5", "0.435");
            KeyPress(vm, "CE", "");
            EnterOp(vm, Operation.Mult, "");
            EnterNumber(vm, ".87.", "0.87");
            EnterOp(vm, Operation.Sub, "0.87");
            KeyPress(vm, "C", "");

            EnterNumber(vm, "2.3..", "2.3");
            KeyPress(vm, "=", "2.3");
            KeyPress(vm, "CE", "");
            KeyPress(vm, "=", "");
            EnterOp(vm, Operation.Div, "");
            EnterNumber(vm, ".50", "0.50");
            KeyPress(vm, "C", "");

            EnterOp(vm, Operation.Sub, "");
            EnterNumber(vm, "3.50");
            KeyPress(vm, "=", "3.5");
            KeyPress(vm, "=", "3.5");
        }
        [TestMethod]
        public void OpInvalidInput()
        {
            var vm = CreateViewModel();

            EnterNumber(vm, "1");
            KeyPress(vm, "CE", "");
            EnterOp(vm, Operation.Mult, "");
            EnterNumber(vm, "92");
            KeyPress(vm, "=", "92");

            EnterOp(vm, Operation.Div, "92");
            EnterNumber(vm, "4");
            KeyPress(vm, "=", "23");
            KeyPress(vm, "=", "5.75");
            KeyPress(vm, "C", "");

            EnterNumber(vm, "32");
            KeyPress(vm, "CE", "");
            EnterOp(vm, Operation.Sub, "");
            EnterNumber(vm, ".87", "0.87");
            KeyPress(vm, "=", "0.87");

            EnterOp(vm, Operation.Mult, "0.87");
            EnterNumber(vm, "2");
            KeyPress(vm, "=", "1.74");
            KeyPress(vm, "=", "3.48");
            KeyPress(vm, "C", "");

            EnterNumber(vm, ".32", "0.32");
            KeyPress(vm, "CE", "");
            EnterOp(vm, Operation.Sub, "");
            EnterNumber(vm, ".87", "0.87");
            KeyPress(vm, "=", "0.87");

            EnterOp(vm, Operation.Mult, "0.87");
            EnterNumber(vm, "2");
            KeyPress(vm, "=", "1.74");
            KeyPress(vm, "=", "3.48");
        }
        [TestMethod]
        public void NegateBasic()
        {
            var vm = CreateViewModel();

            KeyPress(vm, "-", "-");
            KeyPress(vm, "-", "");
            KeyPress(vm, "-", "-");
            KeyPress(vm, "=", "-");

            EnterOp(vm, Operation.Div, "-");
            EnterNumber(vm, "3.2.41.", "-3.241");
            KeyPress(vm, "=", "-3.241");

            KeyPress(vm, "-", "-");
            KeyPress(vm, "-", "");
            KeyPress(vm, "=", "");
            KeyPress(vm, "=", "");

            EnterOp(vm, Operation.Mult, "");
            EnterNumber(vm, ".12.4", "0.124");
            EnterOp(vm, Operation.Div, "0.124");
            EnterNumber(vm, ".50.", "0.50");
            KeyPress(vm, "-", "-0.50");
            KeyPress(vm, "=", "-0.248");
            KeyPress(vm, "=", "0.496");
            KeyPress(vm, "=", "-0.992");
            EnterOp(vm, Operation.Add, "-0.992");
            KeyPress(vm, "=", "-0.992");

            KeyPress(vm, "-", "-");
            EnterNumber(vm, ".12.", "-0.12");
            EnterOp(vm, Operation.Mult, "-0.12");
            EnterNumber(vm, "1.0000...", "1.0000");
            KeyPress(vm, "-", "-1.0000");
            KeyPress(vm, "-", "1.0000");
            KeyPress(vm, "-", "-1.0000");
            KeyPress(vm, "=", "0.12");
            KeyPress(vm, "=", "-0.12");

            EnterOp(vm, Operation.Div, "-0.12");
            KeyPress(vm, "-", "-");
            EnterNumber(vm, "2.", "-2.");
            KeyPress(vm, "-", "2.");
            KeyPress(vm, "=", "-0.06");
            KeyPress(vm, "=", "-0.03");
        }
        [TestMethod]
        public void NegateOnClear()
        {
            var vm = CreateViewModel();

            KeyPress(vm, "-", "-");
            KeyPress(vm, "=", "-");
            KeyPress(vm, "CE", "");
            KeyPress(vm, "=", "");
            KeyPress(vm, "=", "");

            EnterNumber(vm, ".34.", "0.34");
            EnterOp(vm, Operation.Add, "0.34");
            EnterNumber(vm, "2.3");
            EnterOp(vm, Operation.Sub, "2.64");

            KeyPress(vm, "-", "-");
            KeyPress(vm, "C", "");
            EnterNumber(vm, ".34.", "0.34");
            KeyPress(vm, "C", "");
            KeyPress(vm, "-", "-");
            EnterNumber(vm, ".34.", "-0.34");

            EnterOp(vm, Operation.Mult, "-0.34");
            EnterNumber(vm, "0.5");
            EnterOp(vm, Operation.Sub, "-0.17");
            KeyPress(vm, "-", "-");

            KeyPress(vm, "C", "");
            KeyPress(vm, "=", "");
            KeyPress(vm, "=", "");
        }
        [TestMethod]
        public void NegatePreEvaluate()
        {
            var vm = CreateViewModel();

            KeyPress(vm, "-", "-");
            EnterNumber(vm, "0.2.3", "-0.23");
            EnterOp(vm, Operation.Mult, "-0.23");

            EnterNumber(vm, "0.32.", "0.32");
            KeyPress(vm, "-", "-0.32");
            KeyPress(vm, "CE", "");
            KeyPress(vm, "-", "-");
            EnterOp(vm, Operation.Div, "-");

            EnterNumber(vm, ".50", "-0.50");
            KeyPress(vm, "-", "0.50");
            EnterOp(vm, Operation.Mult, "-0.115");

            KeyPress(vm, "-", "-");
            KeyPress(vm, "-", "");
            EnterOp(vm, Operation.Sub, "");

            KeyPress(vm, "-", "-");
            KeyPress(vm, "-", "");
            EnterNumber(vm, ".32.", "0.32");
            EnterOp(vm, Operation.Div, "-0.435");

            KeyPress(vm, "-", "-");
            KeyPress(vm, "=", "-");
            KeyPress(vm, "=", "-");
            KeyPress(vm, "-", "");

            EnterOp(vm, Operation.Sub, "");
            KeyPress(vm, "-", "-");
            EnterNumber(vm, ".123", "-0.123");
            KeyPress(vm, "=", "-0.312");
            KeyPress(vm, "=", "-0.189");
            KeyPress(vm, "=", "-0.066");
            KeyPress(vm, "=", "0.057");
        }
        [TestMethod]
        public void RepeatZero()
        {
            var vm = CreateViewModel();

            EnterNumber(vm, "000.002.02002", "0.00202002");
            KeyPress(vm, "=", "0.00202002");
            KeyPress(vm, "-", "-");
            EnterNumber(vm, "000.100.0000.02", "-0.100000002");
            KeyPress(vm, "-", "0.100000002");
            KeyPress(vm, "CE", "");
            KeyPress(vm, "-", "-");
            KeyPress(vm, "-", "");
            EnterNumber(vm, ".101..11000.002", "0.10111000002");
            EnterOp(vm, Operation.Mult, "0.10111000002");
            EnterNumber(vm, "100000.000", "100000.000");
            KeyPress(vm, "-", "-100000.000");
            EnterOp(vm, Operation.Sub, "-10111.000002");
            KeyPress(vm, "C", "");
            KeyPress(vm, "-", "-");
            EnterNumber(vm, "000000", "-0");
            KeyPress(vm, "=", "0");
        }
        [TestMethod]
        public void LeadingZero()
        {
            var vm = CreateViewModel();

            EnterNumber(vm, "0001.002.02002", "1.00202002");
            KeyPress(vm, "=", "1.00202002");
            KeyPress(vm, "-", "-");
            EnterNumber(vm, "007.100.0000.02", "-7.100000002");
            KeyPress(vm, "-", "7.100000002");
            KeyPress(vm, "CE", "");
            KeyPress(vm, "-", "-");
            KeyPress(vm, "-", "");
            EnterNumber(vm, ".101..11000.002", "0.10111000002");
            EnterOp(vm, Operation.Mult, "0.10111000002");
            EnterNumber(vm, "100000.000", "100000.000");
            KeyPress(vm, "-", "-100000.000");
            EnterOp(vm, Operation.Sub, "-10111.000002");
            KeyPress(vm, "C", "");
            KeyPress(vm, "-", "-");
            EnterNumber(vm, "000010", "-10");
            KeyPress(vm, "=", "-10");
        }

        private ViewModel CreateViewModel()
        {
            ViewModel vm = new ViewModel();
            Assert.AreEqual("", vm.Display);
            return vm;
        }
        private void EnterNumber(ViewModel vm, string input, string outcome = null)
        {
            foreach (char keypress in input)
            {
                if (keypress == '.')
                {
                    Assert.IsTrue(vm.DecCommand.CanExecute(null));
                    vm.DecCommand.Execute(null);
                }
                else
                {
                    int num = keypress - '0';
                    Assert.IsTrue(vm.NumCommands[num].CanExecute(null));
                    vm.NumCommands[num].Execute(null);
                }
            }

            Assert.AreEqual(outcome ?? input, vm.Display);
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
                    Assert.IsTrue(vm.EvalCommand.CanExecute(null));
                    vm.EvalCommand.Execute(null);
                    break;
                case "-":
                    Assert.IsTrue(vm.NegateCommand.CanExecute(null));
                    vm.NegateCommand.Execute(null);
                    break;
                case "C":
                    Assert.IsTrue(vm.ClearCommand.CanExecute(null));
                    vm.ClearCommand.Execute(null);
                    break;
                case "CE":
                    Assert.IsTrue(vm.ClearEntryCommand.CanExecute(null));
                    vm.ClearEntryCommand.Execute(null);
                    break;
                default:
                    throw new ArgumentException("Operation not supported: " + key, "key");
            }

            if (outcome != null)
                Assert.AreEqual(outcome, vm.Display);
        }
    }
}
