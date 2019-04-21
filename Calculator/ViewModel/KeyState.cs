namespace Calculator.ViewModel
{
    public enum Operation { Add, Sub, Mult, Div, Last = Div };

    class KeyState
    {
        private enum State { Reset, Import, Editing, FirstOp, NextOp, Evaluated, NoChain };
        private enum NegState { Pos, Neg, Clearing, Negating };
        private State state = State.Reset;
        private bool decimalAdded = false;
        private NegState negative = NegState.Pos;
        private bool displayInvalid = false;
        private bool isEvaluate;
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
            get { return !decimalAdded; }
        }
        public bool AddZeroDecimal
        {
            get { return displayInvalid; }
        }
        public bool ClearAccumulator
        {
            get { return state == State.Evaluated ||
                         state == State.Reset     ||
                         state == State.NoChain; }
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
        public bool RemoveNegative
        {
            get
            {
                if (negative == NegState.Negating)
                {
                    negative = NegState.Neg;
                    return false;
                }
                negative = NegState.Pos;
                return true;
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
            get { return (!displayInvalid           ||
                          (negative == NegState.Pos &&
                           state != State.Import))  &&
                           state != State.Reset; }
        }
        public bool UpdateDisplay
        {
            get { return !displayInvalid       &&
                          state != State.Reset &&
                         (isEvaluate     ||
                          state != State.FirstOp); }
        }

        public void OnClear()
        {
            state = State.Reset;
            decimalAdded = false;
            displayInvalid = false;
            negative = NegState.Pos;
        }
        public void OnClearEntry()
        {
            displayInvalid = true;
            decimalAdded = false;
            negative = NegState.Pos;
        }
        public void OnDecimalPost()
        {
            decimalAdded = true;
            displayInvalid = false;
        }
        public void OnEvaluate()
        {
            if (state == State.NextOp || state == State.FirstOp)
                state = State.NoChain;
            isEvaluate = true;
        }
        public void OnNegative()
        {
            if (negative == NegState.Neg)
                negative = NegState.Clearing;
            else
                negative = NegState.Negating;
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
            if (negative == NegState.Negating || negative == NegState.Clearing)
            {
                displayInvalid = true;
                negative = NegState.Neg;
            }
            else
            {
                displayInvalid = false;
                negative = NegState.Pos;
            }

            if (state != State.NextOp && state != State.FirstOp)
            {
                CurrentOperation = Operation.Add;
                state = State.Import;
            }
            else
                state = State.Editing;

            decimalAdded = false;
        }
    }
}
