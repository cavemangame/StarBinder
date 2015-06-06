using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarBinder.Core
{
    class GalaxyMixer
    {
        private readonly Galaxy galaxy;

        public GalaxyMixer(Galaxy galaxy)
        {
            this.galaxy = galaxy;
        }

        public bool Mix(int bestSolveLength, out List<int> solve)
        {
            var states = galaxy.States.ToList();
            var stars = galaxy.Stars.ToList();
            var current = new GalaxyLite(galaxy);
            var max = current.MaxState;
            var initials = new HashSet<int>();
            var complete = false;
            var rnd = new Random();

            solve = new List<int>();

            while (!complete && initials.Count < max)
            {
                var state = rnd.Next(max);
                if (initials.Add(state))
                {
                    current = current.CreateWithNewInitial(state);
                    var resolver = new GalaxyResolver(current);
                    complete = resolver.TryDirect(bestSolveLength, out solve);
                }
            }

            if (!complete) return false;

            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].SetInitialState(states[current.GetStarState(i, current.InitialState)]);
            }

            return true;
        }
    }
}
