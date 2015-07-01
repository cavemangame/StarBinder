using System;
using System.ComponentModel;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using NControl.Abstractions;
using NGraphics;
using StarBinder.WindowsApp.NControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinRT;


[assembly: ExportRenderer(typeof(NControlView), typeof(NControlViewRenderer))]
namespace StarBinder.WindowsApp.NControls
{
    /// <summary>
    /// NControlView renderer.
    /// </summary>
    public class NControlViewRenderer : ViewRenderer<NControlView, NControlNativeView>
    {
        private readonly Windows.UI.Xaml.Controls.Image image;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public NControlViewRenderer() : base()
        {
            image = new Windows.UI.Xaml.Controls.Image() { VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch };
        }

        public static void Init() { }

        /// <summary>
        /// Raises the element changed event.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<NControlView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
                e.OldElement.OnInvalidate -= HandleInvalidate;

            if (e.NewElement != null)
            {
                e.NewElement.OnInvalidate += HandleInvalidate;
            }
            else
            {
                return;
            }

            if (Control == null)
            {
                var ctrl = new NControlNativeView();
                ctrl.Children.Add(image);

                SetNativeControl(ctrl);

                UpdateClip();
                UpdateInputTransparent();
            }

            RedrawControl();
        }

        /// <summary>
        /// Redraw when background color changes
        /// </summary>
        protected override void UpdateBackgroundColor()
        {
            base.UpdateBackgroundColor();

            RedrawControl();
        }

        /// <summary>
        /// Raises the element property changed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null)
                return;

            if (e.PropertyName == Layout.IsClippedToBoundsProperty.PropertyName)
                UpdateClip();

            else if (e.PropertyName == VisualElement.HeightProperty.PropertyName ||
                e.PropertyName == VisualElement.WidthProperty.PropertyName)
            {
                // Redraw when height/width changes
                UpdateClip();
                RedrawControl();
            }
            else if (e.PropertyName == NControlView.BackgroundColorProperty.PropertyName)
                RedrawControl();
            else if (e.PropertyName == NControlView.InputTransparentProperty.PropertyName)
                UpdateInputTransparent();
        }
        
        #region Drawing

        private void RedrawControl()
        {
            if (Element.Width.Equals(-1) || Element.Height.Equals(-1))
                return;

            //var sis = new SurfaceImageSource(200,200);//((int)Element.Width, (int)Element.Height);
            //var canvas = new SurfaceImageSourceCanvas(sis, new Rect(0, 0, 200, 200));//(int)Element.Width, (int)Element.Height));

            var canvas = new WICBitmapCanvas(new NGraphics.Size((int)Element.Width, (int)Element.Height));

            Element.Draw(canvas, new Rect(0, 0, Element.Width, Element.Height));
            var stream = new MemoryStream();
            canvas.GetImage().SaveAsPng(stream);
            stream.Position = 0;
            var bmp = new BitmapImage();
            bmp.SetSource(stream.AsRandomAccessStream());
            image.Source = bmp;
            stream.Dispose();
        }

        #endregion
       
        #region Private Members

        /// <summary>
        /// Updates clic on the element
        /// </summary>
        private void UpdateClip()
        {
            if (Element.Width.Equals(-1) || Element.Height.Equals(-1))
                return;

            Control.SetClip(Element.IsClippedToBounds);
        }

        /// <summary>
        /// Updates the IsHitTestVisible property on the native control
        /// </summary>
        private void UpdateInputTransparent()
        {
            Control.IsHitTestVisible = !Element.InputTransparent;
        }

        /// <summary>
        /// Handles the invalidate.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        private void HandleInvalidate(object sender, EventArgs args)
        {
            // Invalidate control
            RedrawControl();
        }

        #endregion

        #region Static Touch Handler

        /// <summary>
        /// Touch handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected static void Touch_FrameReported(object sender, MouseEventArgs e)
        //{
        //    // Get the primary touch point. We do not track multitouch at the moment.
        //    var primaryTouchPoint = e.(Windows.UI.Xaml.Window.Current.Content);

        //    var uiElements = VisualTreeHelper.FindElementsInHostCoordinates(primaryTouchPoint.Position, System.Windows.Application.Current.RootVisual);
        //    foreach(var uiElement in uiElements)
        //    {
        //        // Are we interested?
        //        //var renderer = uiElement as NControlViewRenderer;
        //        //if (renderer == null)
        //        //    continue;

        //        //// Get NControlView element
        //        //var element = renderer.Element;
            
        //        //// Get this' position on screen
        //        //var transform = System.Windows.Application.Current.RootVisual.TransformToVisual(renderer.Control);

        //        //// Transform touches
        //        //var touchPoints = e.GetTouchPoints(System.Windows.Application.Current.RootVisual);
        //        //var touches = touchPoints
        //        //    .Select(t => transform.Transform(new System.Windows.Point(t.Position.X, t.Position.Y)))
        //        //    .Select(t => new NGraphics.Point(t.X, t.Y)).ToList();
                
        //        //var result = false;
        //        //if (primaryTouchPoint.Action == TouchAction.Move)
        //        //{
        //        //    result = element.TouchesMoved(touches);
        //        //}
        //        //else if (primaryTouchPoint.Action == TouchAction.Down)
        //        //{
        //        //    result = element.TouchesBegan(touches);
        //        //}
        //        //else if (primaryTouchPoint.Action == TouchAction.Up)
        //        //{
        //        //    result = element.TouchesEnded(touches);
        //        //}

        //        //if (result)
        //        //    break;
        //    }
        //}
        #endregion
    }
}
