using System.Collections.Generic;
using System.Linq;

namespace StarBinder.Core
{
    public struct Point<T>
    {
        public readonly T X;
        public readonly T Y;

        public Point(T x, T y)
        {
            X = x;
            Y = y;
        }
    }
    
    public class SizeCalculator
    {
        private readonly int width;
        private readonly int height;

        public SizeCalculator(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public Point<int>[] GetCocosPoints(Star star)
        {
            //todo брать путь из ресурсов
            var pts = TestStarPath().Select(p => new Point<int>((int)((star.XRel + star.WRel*p.X)*width), (int)((1 - (star.YRel + star.HRel*p.Y))*height)));
            return pts.ToArray();
        }

        private IEnumerable<Point<double>> TestStarPath()
        {
            yield return new Point<double>(0, -1);
            yield return new Point<double>(-4.0/18, -6.0/18);
            yield return new Point<double>(-1, -6.0/18);
            yield return new Point<double>(-6.0/18, 4.0/18);
            yield return new Point<double>(-10.0/18, 16.0/18);
            yield return new Point<double>(0, 6.0/18);
            yield return new Point<double>(10.0/18, 16.0/18);
            yield return new Point<double>(6.0/18, 4.0/18);
            yield return new Point<double>(1, -6.0/18);
            yield return new Point<double>(6.0/18, -6.0/18);
        }
    }
}
