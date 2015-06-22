using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NControl.Abstractions;
using NGraphics;
using StarBinder.Core;
using Xamarin.Forms;
using PathOp = NGraphics.PathOp;
using Point = NGraphics.Point;
using Color = NGraphics.Color;

namespace StarBinder.Controls
{
    public class GameControl : NControlView
    {
        private readonly SizeCalculator calculator = new SizeCalculator();

        public GameControl()
        {
            VerticalOptions = LayoutOptions.Fill;
            HorizontalOptions = LayoutOptions.Fill;
        }
        
        public override void Draw(ICanvas canvas, Rect rect)
        {
            calculator.Resize((int)rect.Width, (int)rect.Height);
            base.Draw(canvas, rect);

            if (Links != null)
            foreach (var link in Links)
            {
                var from = link.From.LinkPoint(calculator);
                var to = link.To.LinkPoint(calculator);
                //var points = new PathOp[2];
                //points[0] = new MoveTo(from);
                //points[1] = new LineTo(to);
                //var gradient = new LinearGradientBrush(from, to, link.From.State.Color.ToColor(), link.To.State.Color.ToColor()) { Absolute = true };
                //var path = new Path(points)
                //{
                //    Pen = new Pen("Red", 2)
                //};
                //path.Brush = gradient;
                //path.Draw(canvas);

                //canvas.DrawPath(points, new Pen(Colors.Clear, 2), gradient);
                canvas.DrawLine(from, to, 4, link.From.State.Color.ToColor(), link.To.State.Color.ToColor());
            }
        }

        public static BindableProperty LinksProperty = BindableProperty.Create<GameControl, IEnumerable<Link>>(
            p => p.Links, default(IEnumerable<Link>), BindingMode.OneWay, null, LinksChanged);

        public IEnumerable<Link> Links
        {
            get { return (IEnumerable<Link>)GetValue(LinksProperty); }
            set { SetValue(LinksProperty, value); }
        }

        private static void LinksChanged(BindableObject bindable, IEnumerable<Link> oldValue, IEnumerable<Link> newValue)
        {
            ((GameControl)bindable).Invalidate();
        }
    }

    static class DrawingExtensions
    {
        public static Point LinkPoint(this Star star, SizeCalculator calculator)
        {
            return new Point(calculator.XRelToAbs(star.XRel), calculator.YRelToAbs(star.YRel));
        }

		public static Color ToColor(this string hexString)
		{
			return Color.FromRGB(int.Parse(hexString.Replace("#", ""), NumberStyles.HexNumber));
		}

        public static void DrawLine(this ICanvas canvas, Point from, Point to, double width, Color color1, Color color2)
        {
            var hw = width > 1 ? width/2 : 0.5;
            var pt1 = new Point(from.X < to.X ? from.X - hw : from.X + hw, from.Y);
            var pt2 = new Point(from.X, from.Y < to.Y ? from.Y - hw : from.Y + hw);
            var pt3 = new Point(to.X < from.X ? to.X - hw : to.X + hw, to.Y);
            var pt4 = new Point(to.X, to.Y < from.Y ? to.Y - hw : to.Y + hw);
            var gradient = new LinearGradientBrush(from, to, color1, color2) { Absolute = true };

            canvas.FillPath(new PathOp[] { new MoveTo(pt1), new LineTo(pt2), new LineTo(pt3), new LineTo(pt4), new ClosePath() }, gradient);
        }
    }
}
