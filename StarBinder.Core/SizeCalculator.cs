﻿using System;
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

	public struct Rect<T>
	{
		public readonly Point<T> Point;
		public readonly T Width;
		public readonly T Height;

		public Rect(Point<T> p, T w, T h)
		{
			Point = p;
			Width = w;
			Height = h;
		}
	}

    public static class PointExtensions
    {
        public static Point<double> Multiply (this Point<double> point, double coeff)
        {
            return new Point<double>(point.X * coeff, point.Y * coeff);
        }

        public static Point<double> Rotate(this Point<double> point, double angle)
        {
            var radians = angle*Math.PI/180;
            return new Point<double>(point.X * Math.Cos(radians) - point.Y * Math.Sin(radians), 
                                     point.X * Math.Sin(radians) + point.Y * Math.Cos(radians));
        }
    }
    
    public class SizeCalculator
    {
        private int width;
        private int height;

        public SizeCalculator()
        {
        }

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

        public void Resize(double width, double height)
        {
            this.width = (int)width;
            this.height = (int)height;
        }

        public double XAbsToRel(double abs)
        {
            return abs/width;
        }

        public double YAbsToRel(double abs)
        {
            return abs/height;
        }

        public int XRelToAbs(double rel)
        {
            return (int)(rel * width);
        }

        public int YRelToAbs(double rel)
        {
            return (int)(rel * height);
        }

        public int RelToAbsByMinSize(double relative)
        {
            var size = Math.Min(width, height);
            return (int)(size * relative);
        }

		public int RelToAbsByMaxSize(double relative)
		{
			var size = Math.Max(width, height);
			return (int)(size * relative);
		}

        public double AbsToRelByMinSize(double abs)
        {
            var size = Math.Min(width, height);
            return abs/size;
        }

        public Point<int>[] GetCocosPoints(Star star, bool isBack)
        {
            return GetStarPoints(star, isBack)
                    .Select(p => new Point<int>(RelToAbsByMinSize(star.XRel + star.HalfWidthRel * p.X),
                                                RelToAbsByMinSize(1 - star.YRel - star.HalfWidthRel * p.Y)))
                    .ToArray();
        }

        public Point<double>[] GetAbsoluteStarPoints(Star star, bool isBack)
        {
            return GetStarPoints(star, isBack).Select(p => new Point<double>(
                    RelToAbsByMinSize(star.HalfWidthRel * p.X) + XRelToAbs(star.XRel),
                    RelToAbsByMinSize(star.HalfWidthRel * p.Y) + YRelToAbs(star.YRel)
                )).ToArray();
        }

        public Point<double>[] GetStreachedStarPoints(Star star, bool isBack)
        {
            return GetStarPoints(star, isBack).Select(p => new Point<double> (
                width * (p.X + 1)/2, height * (p.Y + 1)/2
            )).ToArray();
        }

        public Point<int>[] GetWpfPoints(Star star, bool isBack)
        {
            return GetStarPoints(star, isBack).Select(p => new Point<int>(
                RelToAbsByMinSize(star.HalfWidthRel * p.X), 
                RelToAbsByMinSize(star.HalfWidthRel * p.Y))).ToArray();
        }

        public Point<int>[] GetWinphonePoints(Star star, bool isBack)
        {
            return GetStarPoints(star, isBack).Select(p => new Point<int>(
                RelToAbsByMinSize(star.HalfWidthRel * (p.X + 1)), 
                RelToAbsByMinSize(star.HalfWidthRel * (p.Y + 1)))).ToArray();
        }

        private IEnumerable<Point<double>> GetStarPoints(Star star, bool isBack)
        {
            var num = star.IsSubBeams ? star.Beams*4 : star.Beams*2;
            var angle = 2 * Math.PI/num;
            var sbRad = star.InnerCoeff + (1 - star.InnerCoeff) * star.SubBeamsCoeff;
            var scale = isBack ? 1 : star.FrontScale;
            var rotate = isBack ? star.RotateAngle : star.RotateAngle + star.FrontAngle;

            for (var i = 0; i < num; i++)
            {
                var point = new Point<double>(Math.Cos(i * angle), Math.Sin(i * angle)).Multiply(scale).Rotate(rotate);
                
                if (i % 2 == 1)
                {
                    yield return point.Multiply(star.InnerCoeff);
                }
                else if (star.IsSubBeams && i % 4 == 2)
                {
                    yield return point.Multiply(sbRad);
                }
                else
                {
                    yield return point;
                }
            }

            yield return new Point<double>(1, 0).Multiply(scale).Rotate(rotate);
        }

		public Rect<float> RectRelToAbsByMinSize(float x, float y, float w, float h)
		{
			float min = Math.Min (width, height);
			return new Rect<float> (new Point<float> (x * min, y * min), w * min, h * min);
		}
    }
}
