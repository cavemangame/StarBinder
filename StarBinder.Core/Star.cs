using System.Collections.Generic;

namespace StarBinder.Core
{
    public class Star : ViewObject
    {
        private State state;
        private State finalState;
        private State initialState;

        internal Star(State initialState, State finalState)
        {
            State = initialState;
            InitialState = initialState;
            FinalState = finalState;
            Links = new List<Link>();

            //default values 
            WRel = 0.25;
            XRel = 0.25;
            YRel = 0.25;

            Beams = 4;
            IsSubBeams = true;
            InnerCoeff = 0.3;
            SubBeamsCoeff = 0.5;
            RotateAngle = 15;
        }
        
        public double XRel { get; set; }
        public double YRel { get; set; }
        public double WRel { get; set; }

        public int Beams { get; set; }
        public bool IsSubBeams { get; set; }
        public double InnerCoeff { get; set; }
        public double SubBeamsCoeff { get; set; }
        public int RotateAngle { get; set; }

        public IEnumerable<Link> Links { get; private set; }

        public State State
        {
            get { return state; }
            private set
            {
                if (state == value) return;
                state = value;
                OnPropertyChanged();
            }
        }

        public State FinalState
        {
            get { return finalState; }
            private set
            {
                if (finalState == value) return;
                finalState = value;
                OnPropertyChanged();
            }
        }

        public State InitialState
        {
            get { return initialState; }
            private set
            {
                if (initialState == value) return;
                initialState = value;
                OnPropertyChanged();
            }
        }

        internal void OnStateRemoved(State stateForRemove, State replaceBy)
        {
            if (State == stateForRemove) 
                State = replaceBy;
            
            if (FinalState == stateForRemove) 
                FinalState = replaceBy;

            if (InitialState == stateForRemove)
                InitialState = replaceBy;
        }

        public void NextInitialState()
        {
            InitialState = InitialState.Next;
            State = InitialState;
        }

        public void NextFinalState()
        {
            FinalState = FinalState.Next;
        }

        public void ChangeAll()
        {
            State = State.Next;
            foreach (var link in Links)
            {
                link.ChangeBy(this);
            }
        }

        public void ChangeSingle()
        {
            State = State.Next;
        }

        public void RevertAll()
        {
            State = State.Prevous;
            foreach (var link in Links)
            {
                link.RevertBy(this);
            }
        }

        public void RevertSingle()
        {
            State = State.Prevous;
        }
    }
}
