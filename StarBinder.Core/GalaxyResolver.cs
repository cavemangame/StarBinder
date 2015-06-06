using System.Collections.Generic;
using System.Linq;

namespace StarBinder.Core
{
    class GalaxyResolver
    {
        private readonly GalaxyLite galaxy;
        private readonly Dictionary<int, StateInfo> allStates;

        private int maxSteps;

        public GalaxyResolver(Galaxy galaxy)
        {
            this.galaxy = new GalaxyLite(galaxy);
            allStates = new Dictionary<int, StateInfo>();
        }

        public GalaxyResolver(GalaxyLite galaxy)
        {
            this.galaxy = galaxy;
            allStates = new Dictionary<int, StateInfo>();
        }

        public bool TryWithMax(int max, out List<int> solve)
        {
            solve = new List<int>();
            maxSteps = max;
            ProcessState(new StateInfo(galaxy.InitialState, new int[0]));

            if (!allStates.ContainsKey(galaxy.FinalState)) return false;

            solve = allStates[galaxy.FinalState].Clicks.ToList();
            return true;
        }

        public bool TryDirect(int steps, out List<int> solve)
        {
            solve = new List<int>();

            if (ProcessState(new StateInfo(galaxy.InitialState, new int[0]), steps)) return false;
            if (!allStates.ContainsKey(galaxy.FinalState)) return false;

            solve = allStates[galaxy.FinalState].Clicks.ToList();
            return true;
        }

        //true если решение короче чем нужно
        private bool ProcessState(StateInfo stateInfo, int direct)
        {
            if (galaxy.FinalState == stateInfo.State && stateInfo.ClickCount < direct) return true; //это решение слишком короткое

            StateInfo prevInfo;
            if (allStates.TryGetValue(stateInfo.State, out prevInfo) && prevInfo.ClickCount <= stateInfo.ClickCount) return false;

            allStates[stateInfo.State] = stateInfo;
            
            if (stateInfo.ClickCount < direct && stateInfo.State != galaxy.FinalState)
            {
                for (int i = 0; i < galaxy.StarsCount; i++)
                {
                    var state = galaxy.StarClick(i, stateInfo.State);
                    var clicks = new int[stateInfo.ClickCount + 1];
                    stateInfo.Clicks.CopyTo(clicks, 0);
                    clicks[stateInfo.ClickCount] = i;

                    if (ProcessState(new StateInfo(state, clicks), direct)) return true; //завершение рекурсии
                }
            }

            return false;
        }

        private void ProcessState(StateInfo stateInfo)
        {
            StateInfo prevInfo;
            if (allStates.TryGetValue(stateInfo.State, out prevInfo) && prevInfo.ClickCount <= stateInfo.ClickCount)
            {
                return;
            }
            
            allStates[stateInfo.State] = stateInfo;

            if (stateInfo.ClickCount < maxSteps && stateInfo.State != galaxy.FinalState)
            {
                for (int i = 0; i < galaxy.StarsCount; i++)
                {
                    var state = galaxy.StarClick(i, stateInfo.State);
                    var clicks = new int[stateInfo.ClickCount + 1];
                    stateInfo.Clicks.CopyTo(clicks, 0);
                    clicks[stateInfo.ClickCount] = i;

                    ProcessState(new StateInfo(state, clicks));
                }
            }
        }


        struct StateInfo
        {
            public readonly int State;
            public readonly int[] Clicks;

            public int ClickCount { get { return Clicks.Length; } }

            public StateInfo(int state, int[] clicks)
            {
                State = state;
                Clicks = clicks;
            }
        }
    }
}
