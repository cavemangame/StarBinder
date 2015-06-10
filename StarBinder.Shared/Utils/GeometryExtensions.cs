using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using StarBinder.Core;

namespace StarBinder.Utils
{
    public static class GeometryExtensions
    {
        public static PathGeometry ToPathGeometry(this IEnumerable<Point<int>> points)
        {
            var geometry = new PathGeometry();
            var figure = new PathFigure { IsClosed = true };
            var segment = new PolyLineSegment();
            
            foreach (var point in points)
            {
                segment.Points.Add(point.Convert());
            }

            figure.Segments.Add(segment);
            geometry.Figures.Add(figure);
            return geometry;
        }

        public static Point Convert(this Point<int> point)
        {
            return new Point(point.X, point.Y);
        }
    }
}
