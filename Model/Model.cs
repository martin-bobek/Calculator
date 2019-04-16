namespace Calculator.Model
{
    public class Model
    {
        public double Accumulator { get; private set; }
        public double Operand { private get; set; }

        public void ClearAccumulator()
        {
            Accumulator = 0;
        }
        public void Add()
        {
            Accumulator += Operand;
        }
        public void Subtract()
        {
            Accumulator -= Operand;
        }
        public void Multiply()
        {
            Accumulator *= Operand;
        }
        public void Divide()
        {
            Accumulator /= Operand;
        }
    }
}
