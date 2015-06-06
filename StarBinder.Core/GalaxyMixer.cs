using System;
using System.Collections.Generic;
using System.Linq;

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
            var computed = new HashSet<int>();
            var complete = false;
            var rnd = new Random();
            var state = current.InitialState;

            solve = new List<int>();
            
            while (!complete && state != -1)
            {
                current = current.CreateWithNewInitial(state);
                var resolver = new GalaxyResolver(current);
                complete = resolver.TryDirect(bestSolveLength, out solve);
                state = GetNextState(computed, max, rnd);
            }

            if (!complete) return false;

            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].SetInitialState(states[current.GetStarState(i, current.InitialState)]);
            }

            return true;
        }

        private int GetNextState(HashSet<int> computed, int max, Random rnd)
        {
            if (computed.Count == max + 1) return -1;

            int i;

            //если хэш наполовину заполнен, то идем подряд
            if (computed.Count > max >> 1)
            {
                for (i = 0; i <= max; i++)
                {
                    if (computed.Add(i)) return i;
                }
            }

            //выбираем случайное состояние
            while (true)
            {
                i = rnd.Next(max);
                if (computed.Add(i)) return i;
            }
        }
    }
}
