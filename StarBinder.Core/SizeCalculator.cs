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
        private int width;
        private int height;

        public SizeCalculator(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void Resize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public double XAbsToRelative(double abs)
        {
            return abs/width;
        }

        public double YAbsToRelative(double abs)
        {
            return abs/height;
        }

        public int XRelativeToAbs(double abs)
        {
            return (int)(abs * width);
        }

        public int YRelativeToAbs(double abs)
        {
            return (int)(abs * height);
        }

        public Point<int>[] GetCocosPoints(Star star)
        {
            return GetStarPath(star)
                    .Select(p => new Point<int>(XRelativeToAbs(star.XRel + star.WRel * p.X),
                                                YRelativeToAbs(1 - star.YRel - star.HRel * p.Y)))
                    .ToArray();
        }

        public Point<int>[] GetWpfPoints(Star star)
        {
            return GetStarPath(star)
                    .Select(p => new Point<int>(XRelativeToAbs(star.XRel + star.WRel*p.X),
                                                YRelativeToAbs(star.YRel + star.HRel*p.Y)))
                    .ToArray();
        }

        //todo
        private IEnumerable<Point<double>> GetStarPath(Star star)
        {
            return TestStarPath();
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
