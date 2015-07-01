using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
        //private readonly Windows.UI.Xaml.Controls.Image image;
        private readonly ImageBrush brush = new ImageBrush();
        //private readonly static ViewToRendererConverter converter = new ViewToRendererConverter();
        private readonly NControlNativeView nativeView;

        public NControlViewRenderer()
        {
            nativeView = new NControlNativeView { Background = brush };
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
            {
                e.OldElement.OnInvalidate -= HandleInvalidate;
                e.OldElement.SizeChanged -= OnSizeChanged;
            }

            if (e.NewElement != null)
            {
                e.NewElement.OnInvalidate += HandleInvalidate;
                e.NewElement.SizeChanged += OnSizeChanged;
                //nativeView.Content = (UIElement)converter.Convert(e.NewElement.Content, typeof(UIElement), null, Language);
            }
            else
            {
                return;
            }

            if (Control == null)
            {
                SetNativeControl(nativeView);
                UpdateClip();
                UpdateInputTransparent();

                nativeView.PointerPressed += OnPointerPressed;
                nativeView.PointerMoved += OnPointerMoved;
                nativeView.PointerCanceled += OnPointerCancelled;
                nativeView.PointerReleased += OnPointerEnded;
            }
        }

        private double preWidth = -1;
        private double preHeight = -1;
        private void OnSizeChanged(object sender, EventArgs eventArgs)
        {
            if (Element.Width.Equals(preWidth) && Element.Height.Equals(preHeight)) 
                return;
            
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

            if (Control == null)return;

            if (e.PropertyName == Layout.IsClippedToBoundsProperty.PropertyName ||
                e.PropertyName == VisualElement.HeightProperty.PropertyName ||
                e.PropertyName == VisualElement.WidthProperty.PropertyName)
            {
                UpdateClip();
            }
            else if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
                RedrawControl();
            else if (e.PropertyName == VisualElement.InputTransparentProperty.PropertyName)
                UpdateInputTransparent();
        }
        
        #region Drawing

        private void RedrawControl()
        {
            if (Element.Width.Equals(-1) || Element.Height.Equals(-1))
                return;

            preWidth = Element.Width;
            preHeight = Element.Height;

            var size = new NGraphics.Size(Element.Width, Element.Height);

            //var sis = new SurfaceImageSource(size);
            //var canvas = new SurfaceImageSourceCanvas(sis, new Rect(size));
            //Element.Draw(canvas, new Rect(size));
            //brush.ImageSource = sis;
            
            var canvas = new WICBitmapCanvas(size);

            Element.Draw(canvas, new Rect(size));
            using (var stream = new MemoryStream())
            {
                canvas.GetImage().SaveAsPng(stream);
                stream.Position = 0;
                var bmp = new BitmapImage();
                bmp.SetSource(stream.AsRandomAccessStream());
                brush.ImageSource = bmp;
            }

            Debug.WriteLine("redrawing: " + Element);
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


        #region Pointer events

        private void OnPointerPressed(object sender, PointerRoutedEventArgs args)
        {
            nativeView.CapturePointer(args.Pointer);
            Element.TouchesBegan(new[] { args.GetCurrentPoint(nativeView).ToPoint() });
            args.Handled = true;
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs args)
        {
            if (args.Pointer.IsInContact)
            {
                Element.TouchesMoved(new[] { args.GetCurrentPoint(nativeView).ToPoint() });
                args.Handled = true;
            }
        }

        private void OnPointerEnded(object sender, PointerRoutedEventArgs args)
        {
            nativeView.ReleasePointerCapture(args.Pointer);
            Element.TouchesEnded(new[] { args.GetCurrentPoint(nativeView).ToPoint() });
            args.Handled = true;
        }

        private void OnPointerCancelled(object sender, PointerRoutedEventArgs args)
        {
            nativeView.ReleasePointerCapture(args.Pointer);
            Element.TouchesCancelled(new[] { args.GetCurrentPoint(nativeView).ToPoint() });
            args.Handled = true;
        }

        #endregion
    }

    static class WinToNGraphicsExtensions
    {
        public static NGraphics.Point ToPoint(this Windows.UI.Input.PointerPoint point)
        {
            return new NGraphics.Point(point.Position.X, point.Position.Y);
        }
    }
}
