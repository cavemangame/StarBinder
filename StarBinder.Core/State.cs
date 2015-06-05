using System;
using System.Diagnostics;

namespace StarBinder.Core
{
    public class StateData
    {
        public string Id;
        public string Color;

        public StateData()
        {
            
        }

        public StateData(State state)
        {
            Color = state.Color;
            Id = state.Id;
        }
    }
    
    public class State : ViewObject
    {
        private string color;
        private StateData data;

        State()
        {
            Id = Guid.NewGuid().ToString();
        }

        internal static State CreateAfter(State state, string color = null)
        {
            var newState = new State { Next = state.Next, Prevous = state };
            
            state.Next = newState;
            newState.Next.Prevous = newState;
            newState.Color = color ?? state.Color;

            return newState;
        }

        internal static State CreateInitial(string color = null)
        {
            var state = new State();
            state.Next = state;
            state.Prevous = state;
            state.Color = color ?? "#FFFFD700";
            
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


        public string Id { get; private set; }
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
