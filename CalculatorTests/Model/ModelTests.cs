using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Model.Tests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void ClearAccumulator_AccumulatorCleared()
        {
            var model = new Model { Operand = 5 };
            model.Add();
            Assert.AreNotEqual(0, model.Accumulator);

            model.ClearAccumulator();

            Assert.AreEqual(0, model.Accumulator);
        }
        [TestMethod]
        public void Add_AddedToAccumulator()
        {
            double start = 5.6;
            double adder = 7.2;
            double expected = 12.8;
            var model = new Model { Operand = start };
            model.Add();

            model.Operand = adder;
            model.Add();
            double result = model.Accumulator;

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void Subtract_SubtractedFromAccumulator()
        {
            double start = 5.6;
            double sub = 7.2;
            double expected = start - sub;
            var model = new Model { Operand = start };
            model.Add();

            model.Operand = sub;
            model.Subtract();
            double result = model.Accumulator;

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void Multiply_MultipliedAccumulator()
        {
            double start = 5.6;
            double mult = 7.2;
            double expected = start * mult;
            var model = new Model { Operand = start };
            model.Add();

            model.Operand = mult;
            model.Multiply();
            double result = model.Accumulator;

            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void Divide_DividedAccumulator()
        {
            double start = 5.6;
            double div = 7.2;
            double expected = start / div;
            var model = new Model { Operand = start };
            model.Add();

            model.Operand = div;
            model.Divide();
            double result = model.Accumulator;

            Assert.AreEqual(expected, result);
        }
    }
}
