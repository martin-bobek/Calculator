namespace Calculator.Model
{
    public class Model
    {
        public double Accumulator { get; private set; }

        public void ClearAccumulator()
        {
            Accumulator = 0;
        }
        public void Add(double value)
        {
            Accumulator += value;
        }
        public void Subtract(double value)
        {
            Accumulator -= value;
        }
        public void Multiply(double value)
        {
            Accumulator *= value;
        }
        public void Divide(double value)
        {
            Accumulator /= value;
        }
    }
}
