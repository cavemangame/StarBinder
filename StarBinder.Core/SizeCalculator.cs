﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

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

    public static class PointExtension
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

        public double XAbsToRel(double abs)
        {
            return abs/width;
        }

        public double YAbsToRel(double abs)
        {
            return abs/height;
        }

        public int XRelToAbs(double abs)
        {
            return (int)(abs * width);
        }

        public int YRelToAbs(double abs)
        {
            return (int)(abs * height);
        }
        public int RelToAbsByMinSize(double relative)
        {
            var size = Math.Min(width, height);
            return (int)(size * relative);
        }

        public double AbsToRelByMinSize(double abs)
        {
            var size = Math.Min(width, height);
            return abs/size;
        }

        public Point<int>[] GetCocosPoints(Star star, double scale = 1)
        {
            return GetStarPoints(star, scale)
                    .Select(p => new Point<int>(RelToAbsByMinSize(star.XRel + star.WRel/2 * p.X),
                                                RelToAbsByMinSize(1 - star.YRel - star.WRel/2 * p.Y)))
                    .ToArray();
        }

        public Point<int>[] GetWpfPoints(Star star, double scale = 1)
        {
            return GetStarPoints(star, scale).Select(p => new Point<int>(RelToAbsByMinSize(star.WRel * p.X / 2),
                RelToAbsByMinSize(star.WRel * p.Y / 2))).ToArray();
        }

        private IEnumerable<Point<double>> GetStarPoints(Star star, double scale)
        {
            var num = star.IsSubBeams ? star.Beams*4 : star.Beams*2;
            var angle = 2 * Math.PI/num;
            var sbRad = star.InnerCoeff + (1 - star.InnerCoeff) * star.SubBeamsCoeff;

            for (int i = 0; i < num; i++)
            {
                var point = new Point<double>(Math.Cos(i*angle), Math.Sin(i*angle)).Multiply(scale).Rotate(star.RotateAngle);
                
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

            yield return new Point<double>(1, 0).Multiply(scale).Rotate(star.RotateAngle);
        }
    }
}
