using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using StarBinder.Core;

namespace StarBinder.LevelEditor.Utils
{
    public static class GeometryExtensions
    {
        public static PathGeometry ToPathGeometry(this IEnumerable<Point<int>> points)
        {
            var geometry = new PathGeometry();
            var figure = new PathFigure { IsClosed = true };
            figure.Segments.Add(new PolyLineSegment(points.Select(Convert), true));
            geometry.Figures.Add(figure);
            return geometry;
        }

        public static Point Convert(this Point<int> point)
        {
            return new Point(point.X, point.Y);
        }
    }
}
