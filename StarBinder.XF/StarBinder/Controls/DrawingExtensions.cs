using System.Globalization;
using NGraphics;
using StarBinder.Core;

namespace StarBinder.Controls
{
    static class DrawingExtensions
    {
        public static Point LinkPoint(this Star star, SizeCalculator calculator)
        {
            return new Point(calculator.XRelToAbs(star.XRel), calculator.YRelToAbs(star.YRel));
        }

        public static Color ToColor(this string hexString)
        {
            return Color.FromRGB(int.Parse(hexString.Remove(0, 3), NumberStyles.HexNumber));
        }

        public static void ReDrawLink(this ICanvas canvas, Link link, SizeCalculator calculator)
        {
            var from = link.From.LinkPoint(calculator);
            var to = link.To.LinkPoint(calculator);
            canvas.DrawGradientLine(from, to, 4, link.From.Color.ToColor(), link.To.Color.ToColor());
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
    }
}
