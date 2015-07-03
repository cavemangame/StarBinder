using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using NGraphics;
using TextAlignment = NGraphics.TextAlignment;

namespace StarBinder.WindowsApp.NControls
{
    class WinRTCanvas : ICanvas
    {
        private readonly NControlNativeView nativeView;
        private readonly Stack<Transform> savedStates = new Stack<Transform>();
        private Transform currentTransform = NGraphics.Transform.Identity;

        public WinRTCanvas(NControlNativeView nativeView)
        {
            savedStates.Push(NGraphics.Transform.Identity);
            this.nativeView = nativeView;
        }

        public void Clear()
        {
            nativeView.Children.Clear();
            savedStates.Clear();
            savedStates.Push(NGraphics.Transform.Identity);
        }

        private Transform SavedTransforms
        {
            get
            {
                var res = NGraphics.Transform.Identity;
                foreach (var transform in savedStates)
                {
                    res = transform * res;
                }
                return res;
            }
        }

        #region ICanvas implementation

        public void SaveState()
        {
            savedStates.Push(currentTransform);
            currentTransform = NGraphics.Transform.Identity;
        }

        public void Transform(Transform transform)
        {
            currentTransform = currentTransform * transform;
        }

        public void RestoreState()
        {
            if (savedStates.Count == 0)
                throw new InvalidOperationException("No one state has benn saved");
            
            currentTransform = savedStates.Pop();
        }

        public void DrawText(string text, Rect frame, Font font, TextAlignment alignment = TextAlignment.Left, Pen pen = null, Brush brush = null)
        {
            throw new NotImplementedException();
        }

        public void DrawPath(IEnumerable<PathOp> ops, Pen pen = null, Brush brush = null)
        {
            if (pen == null && brush == null)
                return;

            var path = new Windows.UI.Xaml.Shapes.Path();

            if (brush != null)
                path.Fill = brush.ToNative();

            if (pen != null)
            {
                path.Stroke = pen.ToNative();
                path.StrokeThickness = pen.Width;
            }

            var geo = new StringBuilder();
            
            foreach(var op in ops)
            {
                var mt = op as MoveTo;
                if (mt != null)
                {
                    geo.AppendFormat(CultureInfo.InvariantCulture, " M {0},{1}", mt.Point.X, mt.Point.Y);
                    continue;
                }

                var lt = op as LineTo;
                if(lt != null)
                {
                    geo.AppendFormat(CultureInfo.InvariantCulture, " L {0},{1}", lt.Point.X, lt.Point.Y);
                    continue;
                }

                var at = op as ArcTo;
                if (at != null)
                {
                    var p = at.Point;
                    var r = at.Radius;

                    geo.AppendFormat(CultureInfo.InvariantCulture, " A {0},{1} 0 {2} {3} {4},{5}",
                        r.Width, r.Height,
                        at.LargeArc ? 1 : 0,
                        at.SweepClockwise ? 1 : 0,
                        p.X, p.Y);
                    continue;
                }

                var ct = op as CurveTo;
                if(ct != null)
                {
                    var p = ct.Point;
                    var c1 = ct.Control1;
                    var c2 = ct.Control2;
                    geo.AppendFormat(CultureInfo.InvariantCulture, " C {0},{1} {2},{3} {4},{5}",
                        c1.X, c1.Y, c2.X, c2.Y, p.X, p.Y);
                    continue;
                }

                var cp = op as ClosePath;
                if(cp != null)
                {
                    geo.Append(" z");
                    continue;
                }
            }

            // Convert path string to geometry
            var b = new Binding { Source = geo.ToString() };
            BindingOperations.SetBinding(path, Windows.UI.Xaml.Shapes.Path.DataProperty, b);

            path.RenderTransform = (SavedTransforms * currentTransform).ToNative();
            path.VerticalAlignment = VerticalAlignment.Top;
            path.HorizontalAlignment = HorizontalAlignment.Left;
            
            nativeView.Children.Add(path);
        }

        public void DrawRectangle(Rect frame, Pen pen = null, Brush brush = null)
        {
            var rect = new Windows.UI.Xaml.Shapes.Rectangle();
            var offset = pen != null ? pen.Width : 0.0;

            rect.HorizontalAlignment = HorizontalAlignment.Left;
            rect.VerticalAlignment = VerticalAlignment.Top;
            rect.Width = frame.Width + offset;
            rect.Height = frame.Height + offset;

            if (brush != null) 
                rect.Fill = brush.ToNative();

            if (pen != null)
            {
                rect.Stroke = pen.ToNative();
                rect.StrokeThickness = pen.Width;
            }

            var transoform = SavedTransforms * currentTransform * frame.Position.Translate();
            rect.RenderTransform = transoform.ToNative();
            
            nativeView.Children.Add(rect);
        }

        public void DrawEllipse(Rect frame, Pen pen = null, Brush brush = null)
        {
            var ellipse = new Windows.UI.Xaml.Shapes.Ellipse();
            var offset = pen != null ? pen.Width : 0.0;
            ellipse.Width = frame.Width + offset;
            ellipse.Height = frame.Height + offset;
            ellipse.HorizontalAlignment = HorizontalAlignment.Left;
            ellipse.VerticalAlignment = VerticalAlignment.Top;

            if (brush != null)
                ellipse.Fill = brush.ToNative();

            if (pen != null)
            {
                ellipse.Stroke = pen.ToNative();
                ellipse.StrokeThickness = pen.Width;
            }

            var transoform = SavedTransforms * currentTransform * frame.Position.Translate();
            ellipse.RenderTransform = transoform.ToNative();

            nativeView.Children.Add(ellipse);
        }

        public void DrawImage(IImage image, Rect frame, double alpha = 1)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
