using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarBinder.Core
{
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
    
    
    public class GalaxyResolver
    {
        private readonly GalaxyLite galaxy;

        private int maxSteps;
        private Dictionary<int, StateInfo> allStates;


        public GalaxyResolver(Galaxy galaxy)
        {
            this.galaxy = new GalaxyLite(galaxy);
        }

        public int[] Resolve(int max)
        {
            allStates = new Dictionary<int, StateInfo> ();
            maxSteps = max;
            
            ProcessState(new StateInfo(galaxy.InitialState, new int[0]));
            return allStates.ContainsKey(galaxy.FinalState) ? allStates[galaxy.FinalState].Clicks : null;
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
    }
}
