using System.Globalization;
using System.Linq;
using NGraphics;
using StarBinder.Core;

namespace StarBinder.Controls
{
    static class DrawingExtensions
    {
        public static Point GetPoint(this Star star, SizeCalculator calculator)
        {
            return new Point(calculator.XRelToAbs(star.XRel), calculator.YRelToAbs(star.YRel));
        }

        public static Xamarin.Forms.Rectangle GetXfRectangle(this Star star, SizeCalculator calculator)
        {
            var hw = calculator.RelToAbsByMinSize(star.HalfWidthRel);
            var left = new Xamarin.Forms.Point(calculator.XRelToAbs(star.XRel) - hw, calculator.YRelToAbs(star.YRel) - hw);
            var size = new Xamarin.Forms.Size(hw * 2, hw * 2);
            return new Xamarin.Forms.Rectangle(left, size);
        }

        public static Color ToColor(this string hexString)
        {
            return Color.FromRGB(int.Parse(hexString.Remove(0, 3), NumberStyles.HexNumber));
        }

        public static void DrawLink(this ICanvas canvas, Link link, SizeCalculator calculator, double width)
        {
            var from = link.From.GetPoint(calculator);
            var to = link.To.GetPoint(calculator);
            canvas.DrawGradientLine(from, to, width, link.From.Color.ToColor(), link.To.Color.ToColor());
        }

        public static void DrawGradientLine(this ICanvas canvas, Point from, Point to, double width, Color color1, Color color2)
        {
            var hw = width > 1 ? width / 2 : 0.5;
            var pt1 = new Point(from.X < to.X ? from.X - hw : from.X + hw, from.Y);
            var pt2 = new Point(from.X, from.Y < to.Y ? from.Y - hw : from.Y + hw);
            var pt3 = new Point(to.X < from.X ? to.X - hw : to.X + hw, to.Y);
            var pt4 = new Point(to.X, to.Y < from.Y ? to.Y - hw : to.Y + hw);
            var gradient = new LinearGradientBrush(from, to, color1, color2) { Absolute = true };

            canvas.FillPath(new PathOp[] { new MoveTo(pt1), new LineTo(pt2), new LineTo(pt3), new LineTo(pt4), new ClosePath() }, gradient);
        }

        public static void DrawStar(this ICanvas canvas, Star star, SizeCalculator calculator, bool isAbsolute = false)
        {
            DrawStarPart(canvas, star, calculator, true, isAbsolute);
            DrawStarPart(canvas, star, calculator, false, isAbsolute);
        }

        private static void DrawStarPart(ICanvas canvas, Star star, SizeCalculator calculator, bool isBack, bool isAbsolute)
        {
            var pts = isAbsolute
                ? calculator.GetAbsoluteStarPoints(star, isBack).Select(p => new Point(p.X, p.Y)).ToArray()
                : calculator.GetWinphonePoints(star, isBack).Select(p => new Point(p.X, p.Y)).ToArray();

            var pathOps = new PathOp[pts.Length];
            pathOps[0] = new MoveTo(pts[0]);

            for (int i = 1; i < pts.Length - 1; i++)
            {
                pathOps[i] = new LineTo(pts[i]);
            }

            pathOps[pts.Length - 1] = new ClosePath();

            canvas.FillPath(pathOps, isBack ? star.BackColor.ToColor() : star.Color.ToColor());
        }
    }
}
