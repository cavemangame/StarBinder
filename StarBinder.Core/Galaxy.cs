using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarBinder.Core
{
    public class GalaxyData
    {
        public string Name;
        public string Description;
        public int Number;
        public int StepsSilver;
        public int StepsGold;
        public List<int> BestSolve;
        public List<StateData> States;
        public List<StarData> Stars;
        public List<LinkData> Links;

        public GalaxyData()
        {
            
        }

        public GalaxyData(Galaxy galaxy)
        {
            Name = galaxy.Name;
            Description = galaxy.Description;
            Number = galaxy.Number;
            StepsSilver = galaxy.StepsSilver;
            StepsGold = galaxy.StepsGold;
            BestSolve = galaxy.BestSolve.ToList();
            States = new List<StateData>(galaxy.States.Select(state => new StateData(state)));
            Stars = new List<StarData>(galaxy.Stars.Select(star => new StarData(star)));
            Links = new List<LinkData>(galaxy.Links.Select(link => new LinkData(link)));
        }

		public Galaxy CreateGalaxy()
		{
			return Galaxy.Create (this);
		}
    }


    public class Galaxy
    {
        private State firstState;
        private List<Star> stars;
        private List<Link> links;
        private List<int> solve;
        
        Galaxy()
        {
        }

        public Galaxy Clone()
        {
            return Create(new GalaxyData(this));
        }

        public static Galaxy CreateNew()
        {
            return new Galaxy
            {
                firstState = State.CreateInitial(),
                stars = new List<Star>(),
                links = new List<Link>(),
                solve = new List<int>()
            };
        }

        public static Galaxy Create(GalaxyData data)
        {
            var galaxy = new Galaxy
            {
                Name = data.Name, Number = data.Number, Description = data.Description, 
                StepsGold = data.StepsGold, StepsSilver = data.StepsSilver, solve = data.BestSolve
            };

            var statesCache = new Dictionary<string, State>();
            galaxy.firstState = State.CreateInitial(data.States[0].Color);
            statesCache.Add(data.States[0].Id, galaxy.firstState);
            
            for (var i = 1; i < data.States.Count; i++)
            {
                statesCache.Add(data.States[i].Id, galaxy.AddState(null, data.States[i].Color));
            }

            galaxy.stars = new List<Star>(data.Stars.Select(starData => starData.CreateStar(statesCache)));
            galaxy.links = new List<Link>(data.Links.Select(linkData => galaxy.CreateLink(galaxy.stars[linkData.From], galaxy.stars[linkData.To], linkData.Direction)));

            return galaxy;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Number { get; set; }
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
        
        public bool IsComplete { get { return Stars.All(s => s.State == s.FinalState); } }

        public void ResetStarStates()
        {
            foreach (var star in Stars)
            {
                star.Reset();
            }
        }

		public Star Star(int number)
		{
			return Stars.FirstOrDefault(s => s.Number == number);
		}
        
        public Star AddStar(State first = null, State final = null, State current = null)
        {
            var star = new Star(first ?? firstState, final ?? firstState.Prevous, current) { Number = stars.Count };
            AddStar(star);
            return star;
        }

        private void AddStar(Star star)
        {
            stars.Add(star);
        }

        public void RemoveStar(Star star)
        {
            foreach (var link in star.Links.ToList())
            {
                RemoveLink(link);
            }

            stars.Remove(star);

            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].Number = i;
            }
        }

        public State AddState(State previuos = null, string color = null)
        {
            previuos = previuos ?? firstState.Prevous;
            return State.CreateAfter(previuos, color);
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
            var link = CreateLink(source, target, direction);
            links.Add(link);
            return link;
        }

        private Link CreateLink(Star source, Star target, LinkDirection direction = LinkDirection.Both)
        {
            var link = new Link(source, target, direction);
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

        public Task<IEnumerable<int>> Resolve(int maxSteps)
        {
            var resolver = new GalaxyResolver(this);
            
            return Task.Factory.StartNew(() =>
            {
                resolver.TryWithMax(maxSteps, out solve);
                return BestSolve;
            });
        }

        public Task<IEnumerable<int>> Mix(int steps)
        {
            var mixer = new GalaxyMixer(this);

            return Task.Factory.StartNew(() =>
            {
                List<int> result;
                if (mixer.Mix(steps, out result))
                {
                    solve = result;
                    return BestSolve;
                }
                return new int[0];
            });
        }
    }
}
