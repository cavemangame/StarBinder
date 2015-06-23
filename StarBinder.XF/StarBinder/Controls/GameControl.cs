using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;
using NControl.Abstractions;
using NGraphics;
using StarBinder.Core;
using Xamarin.Forms;
using Color = NGraphics.Color;
using Point = NGraphics.Point;

namespace StarBinder.Controls
{
    public class GameControl : NControlView
    {
        private readonly SizeCalculator calculator = new SizeCalculator();
		private readonly AbsoluteLayout layout; 
        
        public GameControl()
        {
            Content = layout = new AbsoluteLayout { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
        }
        
        public override void Draw(ICanvas canvas, Rect rect)
        {
            calculator.Resize((int)rect.Width, (int)rect.Height);
            base.Draw(canvas, rect);

            if (Links == null) return;
            
            foreach (var link in Links)
            {
                canvas.ReDrawLink(link, calculator);
            }
        }

        public void OnStarPressed(Star star)
        {
            var command = Command as Command<Star>;
            if (command == null) return;
            if (command.CanExecute(star)) command.Execute(star);
        }

        public static BindableProperty CommandProperty = BindableProperty.Create<GameControl, ICommand>(p => p.Command, default(ICommand));
        
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static BindableProperty LinksProperty = BindableProperty.Create<GameControl, IEnumerable<Link>>(
            p => p.Links, default(IEnumerable<Link>), BindingMode.OneWay, null, LinksChanged);

        public IEnumerable<Link> Links
        {
            get { return (IEnumerable<Link>)GetValue(LinksProperty); }
            set { SetValue(LinksProperty, value); }
        }

        public static BindableProperty StarsProperty = BindableProperty.Create<GameControl, IEnumerable<Star>>(
            p => p.Stars, default(IEnumerable<Star>), BindingMode.OneWay, null, StarsChanged);

        public IEnumerable<Star> Stars
        {
            get { return (IEnumerable<Star>)GetValue(LinksProperty); }
            set { SetValue(StarsProperty, value); }
        }

        private static void LinksChanged(BindableObject bindable, IEnumerable<Link> oldValue, IEnumerable<Link> newValue)
        {
            ((GameControl)bindable).Invalidate();
        }

        private static void StarsChanged(BindableObject bindable, IEnumerable<Star> oldValue, IEnumerable<Star> newValue)
        {
            var ctrl = (GameControl) bindable;
            ctrl.layout.Children.Clear();

			//By unknown reasons, NControls size and Xamarin.Forms size are different for the same area of display. 
			var xfCalc = new SizeCalculator((int)ctrl.layout.Width, (int)ctrl.layout.Height);
            
            foreach (var star in newValue)
            {
				var starCtrl = new StarControl(star, ctrl.calculator, ctrl.OnStarPressed);
                var hw = xfCalc.RelToAbsByMinSize(star.HalfWidthRel);
                var left = new Xamarin.Forms.Point(xfCalc.XRelToAbs(star.XRel) - hw, xfCalc.YRelToAbs(star.YRel) - hw);
				var size = new Xamarin.Forms.Size(hw * 2, hw * 2);
				var rect = new Xamarin.Forms.Rectangle(left, size);
                
                AbsoluteLayout.SetLayoutBounds(starCtrl, rect);
                                
                ctrl.layout.Children.Add(starCtrl);
            }
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
