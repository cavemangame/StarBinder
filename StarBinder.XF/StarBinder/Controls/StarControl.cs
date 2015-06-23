using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NControl.Abstractions;
using NGraphics;
using StarBinder.Core;
using Xamarin.Forms;
using Point = NGraphics.Point;

namespace StarBinder.Controls
{
    public class StarControl : NControlView
    {
        private readonly Star star;
        private readonly SizeCalculator calculator;
        private readonly Action<Star> onStarPressed;

        public StarControl(Star star, SizeCalculator calculator, Action<Star> onStarPressed)
        {
            this.star = star;
            this.calculator = calculator;
            this.onStarPressed = onStarPressed;
            //star.PropertyChanged += StarOnPropertyChanged;
        }

        //private void StarOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        //{
        //    if (args.PropertyName != "State") return;

        //    this.ScaleTo(1.0, 65, Easing.BounceOut);
        //}

        public override void Draw(ICanvas canvas, Rect rect)
        {
			base.Draw(canvas, rect);
            DrawStar(canvas, true);
            DrawStar(canvas, false);
        }

        private void DrawStar(ICanvas canvas, bool isBack)
        {
			var pts = calculator.GetWinphonePoints(star, isBack).Select(p => new Point(p.X, p.Y)).ToArray();
            var pathOps = new PathOp[pts.Length];
            pathOps[0] = new MoveTo(pts[0]);

            for (int i = 1; i < pts.Length - 1; i++)
            {
                pathOps[i] = new LineTo(pts[i]);
            }

            pathOps[pts.Length - 1] = new ClosePath();

            canvas.FillPath(pathOps, isBack ? star.BackColor.ToColor() : star.Color.ToColor());
        }


        public override bool TouchesBegan(IEnumerable<Point> points)
        {
            base.TouchesBegan(points);
            this.ScaleTo(0.8, 65, Easing.CubicInOut);
            onStarPressed(star);
            return true;
        }

        public override bool TouchesCancelled(IEnumerable<Point> points)
        {
            base.TouchesCancelled(points);
            TouchesEnded(points);
            return true;
        }

        public override bool TouchesEnded(IEnumerable<Point> points)
        {
            base.TouchesEnded(points);
			this.ScaleTo(1.0, 65, Easing.CubicInOut);
            return true;
        }
    }
}
