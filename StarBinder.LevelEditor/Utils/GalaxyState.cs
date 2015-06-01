using StarBinder.LevelEditor.ViewModels;

namespace StarBinder.LevelEditor.Utils
{
    //набор всяких хреновин, логически малосвязных, но лень плодить кучу классов =)
    class GalaxyState
    {
        private readonly GalaxyViewModel galaxy;

        public GalaxyState(GalaxyViewModel galaxy)
        {
            this.galaxy = galaxy;
            IsEditMode = true;
        }

        public StarViewModel InitialDragStar { get; set; }

        public bool IsEditMode { get; set; }

        public double GetdXRel(double shift)
        {
            return shift / galaxy.Width;
        }

        public double GetdYRel(double shift)
        {
            return shift / galaxy.Height;
        }
    }
}
