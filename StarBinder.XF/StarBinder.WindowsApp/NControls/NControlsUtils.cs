using System;
using NGraphics;
using Color = Windows.UI.Color;
using Transform = NGraphics.Transform;

namespace StarBinder.WindowsApp.NControls
{
    static class NControlsUtils
    {
        public static Transform Inverse(this Transform source)
        {
            var det = source.A*source.D - source.B*source.C;
            
            if (det.Equals(0)) throw new InvalidOperationException("Matrix is singular and cannot be inverted.");

            var r = 1.0/det;
            
            return new Transform(source.D * r, -source.B * r, 
                                -source.C * r, source.A * r, 
                                -(source.D * source.E - source.C * source.F) * r, 
                                (source.B * source.E - source.A * source.F) * r);
        }

        public static Windows.UI.Xaml.Media.Brush ToNative(this Brush brush)
        {
            var sb = brush as SolidBrush;
            if (sb != null)
            {
                // Solid brush
                return new Windows.UI.Xaml.Media.SolidColorBrush(new Color
                {
                    A = sb.Color.A,
                    R = sb.Color.R,
                    G = sb.Color.G,
                    B = sb.Color.B
                });
            }

            var lb = brush as LinearGradientBrush;
            if (lb != null)
            {
                // Linear gradient
                var gradStops = new Windows.UI.Xaml.Media.GradientStopCollection();
                var n = lb.Stops.Count;
                if (n >= 2)
                {
                    for (var i = 0; i < n; i++)
                    {
                        var s = lb.Stops[i];
                        gradStops.Add(new Windows.UI.Xaml.Media.GradientStop
                        {
                            Color = new Color
                            {
                                A = s.Color.A,
                                R = s.Color.R,
                                B = s.Color.B,
                                G = s.Color.G,
                            },
                            Offset = s.Offset,
                        });
                    }
                }

                return new Windows.UI.Xaml.Media.LinearGradientBrush(gradStops, 0);
            }

            throw new InvalidOperationException(string.Format("WinRT has not bruah equal to {0}", brush.GetType().Name));
        }

        public static Windows.UI.Xaml.Media.Brush ToNative(this Pen pen)
        {
            return new Windows.UI.Xaml.Media.SolidColorBrush(new Color
            {
                A = pen.Color.A,
                R = pen.Color.R,
                G = pen.Color.G,
                B = pen.Color.B
            });
        }

        public static Windows.UI.Xaml.Media.Transform ToNative(this Transform trans)
        {
            return new Windows.UI.Xaml.Media.MatrixTransform
            {
                Matrix = new Windows.UI.Xaml.Media.Matrix(trans.A, trans.B, trans.C, trans.D, trans.E, trans.F)
            };
        }

        public static Transform Translate(this Point point)
        {
            return new Transform(1, 0, 0, 1, point.X, point.Y);
        }
    }
}
