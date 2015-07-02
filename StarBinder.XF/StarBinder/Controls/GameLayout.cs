using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using NControl.Abstractions;
using NGraphics;
using StarBinder.Core;
using StarBinder.Resources;
using Xamarin.Forms;
using Point = NGraphics.Point;
using Rectangle = Xamarin.Forms.Rectangle;

namespace StarBinder.Controls
{
    public class GameLayout : AbsoluteLayout
    {
        private readonly SizeCalculator calculator = new SizeCalculator();
        private LinksControl links;
        private List<StarControl> stars;
        
        public void OnStarPressed(Star star)
        {
            var command = Command as Command<Star>;
            if (command == null) return;
            if (command.CanExecute(star)) command.Execute(star);

            RefreshLevelState();
        }

        public static BindableProperty CommandProperty = BindableProperty.Create<GameLayout, ICommand>(p => p.Command, default(ICommand));
        
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static BindableProperty LevelProperty = BindableProperty.Create<GameLayout, Galaxy>(
            p => p.Level, default(Galaxy), BindingMode.OneWay, null, LevelChanged);

        public Galaxy Level
        {
            get { return (Galaxy)GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }
        private static void LevelChanged(BindableObject bindable, Galaxy oldValue, Galaxy newValue)
        {
            var game = (GameLayout)bindable;
            game.OnLevelChanged(oldValue, newValue);
        }

        public static BindableProperty BackImageProperty = BindableProperty.Create<GameLayout, string>(
            p => p.BackImage, default(string), BindingMode.OneWay, null, BackImageChanged);

        public string BackImage
        {
            get { return (string)GetValue(BackImageProperty); }
            set { SetValue(BackImageProperty, value); }
        }

        private Graphic backImage;

        private static void BackImageChanged(BindableObject bindable, string oldValue, string newValue)
        {
            var ctrl = (GameLayout)bindable;
            using (var reader = new StringReader(newValue))
            {
                var svg = new SvgReaderEx(reader);
                ctrl.backImage = svg.Graphic;
            }
        }

        private void OnLevelChanged(Galaxy oldLevel, Galaxy newLevel)
        {
            if (oldLevel != null)
            {
                oldLevel.StateReseted -= OnStateReseted;
            }
            if (newLevel != null)
            {
                RedrawLevel(newLevel, backImage);
                newLevel.StateReseted += OnStateReseted;
            }
        }

        private void OnStateReseted(object sender, EventArgs eventArgs)
        {
            RefreshLevelState();
        }

        private void RedrawLevel(Galaxy galaxy, Graphic back)
        {
            Children.Clear();
            stars = new List<StarControl>();
            
            var xfCalc = new SizeCalculator((int)Width, (int)Height);

            var backCtrl = new BackControl(back, calculator);
            SetLayoutBounds(backCtrl, new Rectangle(0, 0, Width, Height));
            Children.Add(backCtrl);

            links = new LinksControl(galaxy.Links, calculator);
            SetLayoutBounds(links, new Rectangle(0, 0, Width, Height));
            Children.Add(links);

            stars = new List<StarControl>();

            foreach (var star in galaxy.Stars)
            {
                var starCtrl = new StarControl(star, calculator, OnStarPressed);
                stars.Add(starCtrl);
                SetLayoutBounds(starCtrl, star.GetXfRectangle(xfCalc));
                Children.Add(starCtrl);
            }
        }

        private void RefreshLevelState()
        {
            links.Invalidate();

            foreach (var ctrl in stars)
            {
                ctrl.Invalidate();
            }
        }
    }

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
        }

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

    public class LinksControl : NControlView
    {
        private readonly IEnumerable<Link> links;
        private readonly SizeCalculator calculator;

        public LinksControl(IEnumerable<Link> links, SizeCalculator calculator)
        {
            if (links == null) throw new ArgumentNullException("links");
            if (calculator == null) throw new ArgumentNullException("calculator");
            
            this.links = links;
            this.calculator = calculator;
            
            IsEnabled = false;
        }

        public override void Draw(ICanvas canvas, Rect rect)
        {
            base.Draw(canvas, rect);
            foreach (var link in links)
            {
                canvas.ReDrawLink(link, calculator);
            }
        }
    }

    public class BackControl : NControlView
    {
        private readonly Graphic back;
        private readonly SizeCalculator calculator;

        public BackControl(Graphic back, SizeCalculator calculator)
        {
            if (back == null) throw new ArgumentNullException("back");
            if (calculator == null) throw new ArgumentNullException("calculator");

            this.back = back;
            this.calculator = calculator;

            IsEnabled = false;
        }

        public override void Draw(ICanvas canvas, Rect rect)
        {
            calculator.Resize((int) rect.Width, (int) rect.Height);
            base.Draw(canvas, rect);
            back.Size = rect.Size;
            back.Draw(canvas);
        }
    }
}
