namespace Calculator.ViewModel
{
    public enum Operation { Add, Sub, Mult, Div, Last = Div };

    class KeyState
    {
        private enum State { Reset, Import, Editing, FirstOp, NextOp, Evaluated, NoChain };
        private State state = State.Reset;
        private bool decimalAdded = false;
        private bool displayInvalid = false;
        private bool isEvaluate = false;
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
            get { return (!displayInvalid         &&
                         (state == State.Editing  ||
                          state == State.Import)) ||
                         (isEvaluate              &&
                          state == State.Evaluated); }
        }
        public bool CanStoreOperation
        {
            get { return state != State.Reset; }
        }
        public bool CanStoreOperand
        {
            get { return !displayInvalid         &&
                         (state == State.Editing ||
                          state == State.Import); }
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
            get { return !displayInvalid &&
                         (isEvaluate     ||
                          state != State.FirstOp); }
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
            displayInvalid = false;
        }
        public void OnOperationPerformed()
        {
            if (state == State.Import)
                state = isEvaluate ? State.NoChain : State.FirstOp;
            else
                state = isEvaluate ? State.Evaluated : State.NextOp;
            displayInvalid = false;
        }
        public void OnEvaluate()
        {
            if (state == State.NextOp)
                state = State.NoChain;
            isEvaluate = true;
        }
        public void OnOperation()
        {
            if (state == State.NoChain)
                state = State.NextOp;
            isEvaluate = false;
        }
        public void OnDecimal()
        {
            decimalAdded = true;
            displayInvalid = false;
        }
        public void OnClearEntry()
        {
            displayInvalid = true;
            decimalAdded = false;
        }
        public void OnNumber()
        {
            displayInvalid = false;
        }
    }
}
