using System;
using System.Collections.Generic;
using System.Linq;

namespace StarBinder.Core
{
    public class Galaxy
    {
        private readonly State firstState;
        private readonly List<Star> stars;
        private readonly List<Link> links;
        private readonly List<int> solve;
        
        Galaxy()
        {
            firstState = State.CreateInitial();
            
            stars = new List<Star>();
            links = new List<Link>();
        }

        public static Galaxy CreateNew()
        {
            return new Galaxy();
        }

        public static Galaxy Load()
        {
            throw new NotImplementedException();
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Number { get; private set; }
        public int StepsSilver { get; private set; }
        public int StepsGold { get; private set; }

        public IEnumerable<Star> Stars { get { return stars; } }
        public IEnumerable<Link> Links { get { return links; } }
        public IEnumerable<State> States
        {
            get
            {
                var curState = firstState;
                do
                {
                    yield return curState;
                    curState = curState.Next;
                } while (curState != firstState);
            }
        }

        public IEnumerable<int> BestSolve { get { return solve; }  }

        public Star AddStar()
        {
            var star = new Star(firstState, firstState.Prevous);
            stars.Add(star);
            return star;
        }

        public void RemoveStar(Star star)
        {
            foreach (var link in star.Links)
            {
                RemoveLink(link);
            }

            stars.Remove(star);
        }

        public State AddState(State previuos = null)
        {
            previuos = previuos ?? firstState;
            return State.CreateAfter(previuos);
        }

        public void RemoveState(State state)
        {
            var replace = State.Remove(state);

            foreach (var star in Stars)
            {
                star.OnStateRemoved(state, replace);
            }
        }

        public Link AddLink(Star source, Star target, LinkDirection direction = LinkDirection.Both)
        {
            var link = new Link(source, target, direction);
            links.Add(link);
            source.AddLink(link);
            target.AddLink(link);
            return link;
        }

        public void RemoveLink(Link link)
        {
            link.To.RemoveLink(link);
            link.From.RemoveLink(link);
            links.Remove(link);
        }

        public bool CanAddLink(Star source, Star target)
        {
            return source != target && !source.Links.Any(l => l.From == target || l.To == target);
        }
    }
}
