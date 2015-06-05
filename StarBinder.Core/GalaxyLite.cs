using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace StarBinder.Core
{
    class GalaxyLite
    {
        public readonly int StarsCount;
        public readonly int InitialState;
        public readonly int FinalState;
        
        private readonly int statesCount;
        private readonly int[] powers;
        private readonly int[][] links;

        public GalaxyLite(Galaxy galaxy)
        {
            var stars = galaxy.Stars.ToList();
            var states = galaxy.States.ToList();
            statesCount = states.Count;
            StarsCount = stars.Count;

            powers = new int[StarsCount + 1];
            var prepow = 1;
            for (int i = 0; i < StarsCount + 1; i++)
            {
                powers[i] = prepow;
                prepow *= statesCount;
            }

            for (int i = 0; i < stars.Count; i++)
            {
                InitialState = SetStarState(i, states.IndexOf(stars[i].InitialState), InitialState);
                FinalState = SetStarState(i, states.IndexOf(stars[i].FinalState), FinalState);
            }

            links = new int[StarsCount][];
            for (int i = 0; i < stars.Count; i++)
            {
                var star = stars[i];
                IEnumerable<int> from = star.Links.Where(l => l.From == star).Select(l => l.To.Number);
                IEnumerable<int> to = star.Links.Where(l => l.To == star && l.Direction == LinkDirection.Both).Select(l => l.From.Number);
                links[i] = new[] { i }.Union(from).Union(to).ToArray();
            }
        }

        public int StarClick(int star, int state)
        {
            var result = state;

            for (int i = 0; i < links[star].Length; i++)
            {
                result = NextStarState(links[star][i], result);
            }

            return result;
        }

        private int GetStarState(int star, int galaxyState)
        {
            return galaxyState % powers[star + 1] / powers[star];
        }

        private int SetStarState(int star, int state, int galaxyState)
        {
            return galaxyState - GetStarState(star, galaxyState) * powers[star] + state * powers[star];
        }

        private int NextStarState(int star, int galaxyState)
        {
            var next = GetStarState(star, galaxyState) + 1;

            return next == statesCount
                ? galaxyState - (statesCount - 1) * powers[star]
                : galaxyState + powers[star];
        }
    }
}
