namespace StarBinder.Core
{
    public class LinkData
    {
        public int From;
        public int To;
        public LinkDirection Direction;

        public LinkData()
        {
            
        }

        public LinkData(Link link)
        {
            From = link.From.Number;
            To = link.To.Number;
            Direction = link.Direction;
        }
    }
    
    public enum LinkDirection
    {
        Both, Directed
    }
    
    public class Link
    {
        internal Link(Star from, Star to, LinkDirection direction = LinkDirection.Both)
        {
            From = from;
            To = to;
            Direction = direction;
        }

        public Star From { get; private set; }
        public Star To { get; private set; }
        public LinkDirection Direction { get; set; }
        
        public void ChangeBy(Star initiator)
        {
            if (initiator == From)
            {
                To.ChangeSingle();
            }
            else if (initiator == To && Direction == LinkDirection.Both)
            {
                From.ChangeSingle();
            }
        }

        public void RevertBy(Star initiator)
        {
            if (initiator == From)
            {
                To.RevertSingle();
            }
            else if (initiator == To && Direction == LinkDirection.Both)
            {
                From.RevertSingle();
            }
        }
    }
}

