namespace Calculator.ViewModel
{
    public enum Operation { Add, Sub, Mult, Div, Last = Div };

    class KeyState
    {
        private enum State { Reset, Import, Editing, FirstOp, NextOp, Evaluated, NoChain };
        private State state = State.Reset;
        private bool decimalAdded = false;
        private Operation op;

        public Operation CurrentOperation
        {
            get { return op; }
            set
            {
                if (state == State.Evaluated)
                    state = State.NextOp;
                op = value;
            }
        }

        public bool CanOverwrite
        {
            get { return state == State.Reset     ||
                         state == State.NextOp    ||
                         state == State.FirstOp   ||
                         state == State.Evaluated ||
                         state == State.NoChain; }
        }
        public bool CanPerformOperation
        {
            get { return state == State.Editing ||
                         state == State.Import; }
        }
        public bool CanEvaluate
        {
            get { return state == State.Editing ||
                         state == State.Import  ||
                         state == State.Evaluated; }
        }
        public bool CanStoreOperation
        {
            get { return state != State.Reset; }
        }
        public bool CanStoreOperand
        {
            get { return state == State.Editing ||
                         state == State.Import; }
        }
        public bool CanClearAccumulator
        {
            get { return state == State.Evaluated ||
                         state == State.Reset     ||
                         state == State.NoChain; }
        }
        public bool CanAddDecimal
        {
            get { return !decimalAdded; }
        }
        public bool CanUpdateDisplay
        {
            get { return state != State.FirstOp; }
        }

        public void OnOverwrite()
        {
            if (state != State.NextOp && state != State.FirstOp)
            {
                CurrentOperation = Operation.Add;
                state = State.Import;
            }
            else
                state = State.Editing;
            decimalAdded = false;
        }
        public void OnOperationPerformed()
        {
            if (state == State.Import)
                state = State.FirstOp;
            else
                state = State.NextOp;
        }
        public void OnEvaluateOperation()
        {
            if (state == State.Import)
                state = State.NoChain;
            else
                state = State.Evaluated;

        }
        public void OnEvaluate()
        {
            if (state == State.NextOp)
                state = State.NoChain;
        }
        public void OnOperation()
        {
            if (state == State.NoChain)
                state = State.NextOp;
        }
        public void OnDecimal()
        {
            decimalAdded = true;
        }
    }
}
