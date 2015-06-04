using System;
using System.Collections.Generic;

namespace StarBinder.Core
{
    public class Star : ViewObject
    {
        private State state;
        private State finalState;
        private State initialState;
        private List<Link> links;

        internal Star(State initialState, State finalState)
        {
            State = initialState;
            InitialState = initialState;
            FinalState = finalState;
            links = new List<Link>();

            XRel = 0.25;
            YRel = 0.25;
        }
        
        public double XRel { get; set; }
        public double YRel { get; set; }

        public IEnumerable<Link> Links { get { return links; } }

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

        internal void AddLink(Link link)
        {
            links.Add(link);
        }

        internal void RemoveLink(Link link)
        {
            links.Remove(link);
        }

        #region Star geometry

        private double rotateAngle = 15;
        private double subBeamsCoeff = 0.5;
        private double innerCoeff = 0.3;
        private bool isSubBeams = true;
        private int beams = 4;
        private double halfWidthRel = 0.1;
        private double frontAngle = 2;
        private double frontScale = 0.7;

        public double FrontAngle
        {
            get { return frontAngle; }
            set { frontAngle = value; GeometryChanged(this, null); }
        }

        public double FrontScale
        {
            get { return frontScale; }
            set { frontScale = value; GeometryChanged(this, null); }
        }

        public double HalfWidthRel
        {
            get { return halfWidthRel; }
            set { halfWidthRel = value; GeometryChanged(this, null); }
        }

        public int Beams
        {
            get { return beams; }
            set { beams = value; GeometryChanged(this, null); }
        }

        public bool IsSubBeams
        {
            get { return isSubBeams; }
            set { isSubBeams = value; GeometryChanged(this, null); }
        }

        public double InnerCoeff
        {
            get { return innerCoeff; }
            set { innerCoeff = value; GeometryChanged(this, null); }
        }

        public double SubBeamsCoeff
        {
            get { return subBeamsCoeff; }
            set { subBeamsCoeff = value; GeometryChanged(this, null); }
        }

        public double RotateAngle
        {
            get { return rotateAngle; }
            set { rotateAngle = value; GeometryChanged(this, null); }
        }

        public event EventHandler GeometryChanged = (s, a) => {};

        #endregion
    }
}
