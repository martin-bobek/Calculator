namespace Calculator.ViewModel
{
    public enum Operation { Add, Sub, Mult, Div, Last = Div };

    class KeyState
    {
        private enum State { Reset, Editing, NextOp, Evaluated };
        private State state = State.Reset;
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
            get { return state == State.Reset  ||
                         state == State.NextOp ||
                         state == State.Evaluated; }
        }
        public bool CanPerformOperation
        {
            get { return state == State.Editing; }
        }
        public bool CanEvaluate
        {
            get { return state == State.Editing ||
                         state == State.Evaluated; }
        }
        public bool CanStoreOperation
        {
            get { return state != State.Reset; }
        }
        public bool CanStoreOperand
        {
            get { return state == State.Editing ||
                         state == State.NextOp; }
        }
        public bool CanClearAccumulator
        {
            get { return state == State.Evaluated; }
        }

        public void OnOverwrite()
        {
            if (state != State.NextOp)
                CurrentOperation = Operation.Add;
            state = State.Editing;
        }
        public void OnOperationPerformed()
        {
            state = State.NextOp;
        }
        public void OnEvaluate()
        {
            state = State.Evaluated;
        }
    }
}
