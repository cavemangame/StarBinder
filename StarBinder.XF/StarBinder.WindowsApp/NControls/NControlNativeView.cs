using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace StarBinder.WindowsApp.NControls
{
    public class NControlNativeView : Panel
    {
        public NControlNativeView()
        {
            //HorizontalAlignment = HorizontalAlignment.Stretch;
            //VerticalAlignment = VerticalAlignment.Stretch;
            
            Loaded += (sender, args) => SetClip(IsClipped);
            SizeChanged += (sender, args) => SetClip(IsClipped);
        }

        //public UIElement Content
        //{
        //    get { return Children[0]; }
        //    set
        //    {
        //        Children.Clear();
        //        if (value != null) Children.Add(value);
        //    }
        //}

        public bool IsClipped { get; set; }

        public void SetClip(bool clip)
        {
            IsClipped = clip;
            Clip = clip ?
                new RectangleGeometry { Rect = new Rect(0, 0, this.ActualWidth, this.ActualHeight) } :
                null;
        }
    }
}
