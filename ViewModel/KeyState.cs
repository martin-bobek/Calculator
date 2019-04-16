namespace Calculator.ViewModel
{
    class KeyState
    {
        private enum State { Reset, Editing, Evaluated };
        private State state = State.Reset;

        public bool CanOverwrite
        {
            get
            {
                return state == State.Reset ||
                       state == State.Evaluated;
            }
        }
        public bool CanPerformOperation
        {
            get { return state == State.Editing; }
        }
        public bool CanStoreOperation
        {
            get { return state != State.Reset; }
        }

        public void OnOverwrite()
        {
            state = State.Editing;
        }
        public void OnOperationPerformed()
        {
            state = State.Evaluated;
        }
    }
}
