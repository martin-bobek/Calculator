namespace Calculator.ViewModel
{
    public enum Operation { Add, Sub, Mult, Div, Last = Div };

    class KeyState
    {
        private enum State { Reset, Import, Editing, FirstOp, NextOp, Evaluated, NoChain };
        //        private enum DecState { None, Adding, Added };
        private State state = State.Reset;
        //        private DecState decimalAdded = DecState.None;
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

        public bool AddDecimal
        {
            get
            {
                return !decimalAdded;
                /*
                if (decimalAdded == DecState.Adding)
                {
                    decimalAdded = DecState.Added;
                    return true;
                }
                return false;
                */
            }
        }
        public bool ClearAccumulator
        {
            get
            {
                return state == State.Evaluated ||
                       state == State.Reset ||
                       state == State.NoChain;
            }
        }
        public bool OverwriteDisplay
        {
            get
            {
                if (state == State.Reset     ||
                    state == State.NextOp    ||
                    state == State.FirstOp   ||
                    state == State.Evaluated ||
                    state == State.NoChain)
                {
                    OnOverwrite();
                    return true;
                }
                return false;
            }
        }
        public bool PerformOperation
        {
            get
            {
                if ((!displayInvalid          &&
                     (state == State.Editing  ||
                      state == State.Import)) ||
                     (isEvaluate              &&
                      state == State.Evaluated))
                {
                    OnOperationPerformed();
                    return true;
                }
                return false;
            }
        }
        public bool StoreOperand
        {
            get
            {
                return !displayInvalid &&
                       (state == State.Editing ||
                        state == State.Import);
            }
        }
        public bool StoreOperation
        {
            get { return state != State.Reset; }
        }
        public bool UpdateDisplay
        {
            get { return !displayInvalid &&
                         (isEvaluate     ||
                          state != State.FirstOp); }
        }

        public void OnClearEntry()
        {
            displayInvalid = true;
            //decimalAdded = DecState.None;
            decimalAdded = false;
        }
        public void OnDecimal()
        {
            /*
            if (decimalAdded == DecState.None)
                decimalAdded = DecState.Adding;
            */
            decimalAdded = true;
            displayInvalid = false;
        }
        public void OnEvaluate()
        {
            if (state == State.NextOp || state == State.FirstOp)
                state = State.NoChain;
            isEvaluate = true;
        }
        public void OnNumber()
        {
            displayInvalid = false;
        }
        public void OnOperation()
        {
            if (state == State.NoChain)
                state = State.NextOp;
            isEvaluate = false;
        }
        private void OnOperationPerformed()
        {
            if (state == State.Import)
                state = isEvaluate ? State.NoChain : State.FirstOp;
            else
                state = isEvaluate ? State.Evaluated : State.NextOp;
            displayInvalid = false;
        }
        private void OnOverwrite()
        {
            if (state != State.NextOp && state != State.FirstOp)
            {
                CurrentOperation = Operation.Add;
                state = State.Import;
            }
            else
                state = State.Editing;
            /*
            if (decimalAdded != DecState.Adding)
                decimalAdded = DecState.Added;
            else
                decimalAdded = DecState.None;
            */
            decimalAdded = false;
            displayInvalid = false;
        }
    }
}
