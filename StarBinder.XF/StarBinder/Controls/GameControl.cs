using System.IO;
using System.Windows.Input;
using NControl.Abstractions;
using NGraphics;
using StarBinder.Core;
using StarBinder.Resources;
using Xamarin.Forms;
using Point = Xamarin.Forms.Point;
using Rectangle = Xamarin.Forms.Rectangle;
using Size = Xamarin.Forms.Size;

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
            if (backImage == null) return;
            backImage.Size = rect.Size;
            backImage.Draw(canvas);
        }

        public void OnStarPressed(Star star)
        {
            var command = Command as Command<Star>;
            if (command == null) return;
            if (command.CanExecute(star)) command.Execute(star);

            Device.OnPlatform(iOS: () => RedrawLevel(Level));
        }

        public static BindableProperty CommandProperty = BindableProperty.Create<GameControl, ICommand>(p => p.Command, default(ICommand));
        
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static BindableProperty LevelProperty = BindableProperty.Create<GameControl, Galaxy>(
            p => p.Level, default(Galaxy), BindingMode.OneWay, null, LevelChanged);

        public Galaxy Level
        {
            get { return (Galaxy)GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }
        private static void LevelChanged(BindableObject bindable, Galaxy oldValue, Galaxy newValue)
        {
            var ctrl = (GameControl)bindable;
            ctrl.RedrawLevel(newValue);
        }


        public static BindableProperty BackImageProperty = BindableProperty.Create<GameControl, string>(
            p => p.BackImage, default(string), BindingMode.OneWay, null, BackImageChanged);

        public string BackImage
        {
            get { return (string)GetValue(BackImageProperty); }
            set { SetValue(BackImageProperty, value); }
        }

        private Graphic backImage;

        private static void BackImageChanged(BindableObject bindable, string oldValue, string newValue)
        {
            var ctrl = (GameControl)bindable;
            using (var reader = new StringReader(newValue))
            {
                var svg = new SvgReaderEx(reader);
                ctrl.backImage = svg.Graphic;
            }

            ctrl.Invalidate();
        }

        private void RedrawLevel(Galaxy galaxy)
        {
            layout.Children.Clear();

            if (galaxy == null) return;

            var xfCalc = new SizeCalculator((int)layout.Width, (int)layout.Height);

            foreach (var link in galaxy.Links)
            {
                layout.Children.Add(new LinkControl(link, calculator));
            }
            
            foreach (var star in galaxy.Stars)
            {
                var starCtrl = new StarControl(star, calculator, OnStarPressed);
                var hw = xfCalc.RelToAbsByMinSize(star.HalfWidthRel);
                var left = new Point(xfCalc.XRelToAbs(star.XRel) - hw, xfCalc.YRelToAbs(star.YRel) - hw);
                var size = new Size(hw * 2, hw * 2);
                var rect = new Rectangle(left, size);

                AbsoluteLayout.SetLayoutBounds(starCtrl, rect);

                layout.Children.Add(starCtrl);
            }
        }
    }

    public class LinkControl : NControlView
    {
        private readonly Link link;
        private readonly SizeCalculator calculator;

        public LinkControl(Link link, SizeCalculator calculator)
        {
            this.link = link;
            this.calculator = calculator;
            IsEnabled = false;
        }

        public override void Draw(ICanvas canvas, Rect rect)
        {
            base.Draw(canvas, rect);
            canvas.ReDrawLink(link, calculator);
        }
    }
}
