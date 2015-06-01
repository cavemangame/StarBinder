using System;

namespace StarBinder.Core
{
    public class State : ViewObject
    {
        private string color;

        State()
        {
        }

        internal static State CreateAfter(State state)
        {
            var newState = new State { Next = state.Next, Prevous = state };
            
            state.Next = newState;
            newState.Next.Prevous = newState;

            return newState;
        }

        internal static State CreateInitial()
        {
            var state = new State();
            state.Next = state;
            state.Prevous = state;
            state.Color = "#FFFFD700"; //todo
            
            return state;
        }

        internal static State Remove(State state)
        {
            if (state.Next == state)
                throw new InvalidOperationException("Can't remove single state");

            state.Next.Prevous = state.Prevous;
            state.Prevous.Next = state.Next;
            
            return state.Prevous;
        }

        public State Prevous { get; private set; }
        public State Next { get; private set; }

        public string Color
        {
            get { return color; }
            set
            {
                if (color == value) return;
                color = value;
                OnPropertyChanged();
            }
        }
    }
}
