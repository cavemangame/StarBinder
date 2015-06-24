using System.Collections.Generic;
using System.Windows.Input;
using NControl.Abstractions;
using NGraphics;
using StarBinder.Core;
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

            if (Links == null) return;
#if __IOS__  
#else
            foreach (var link in Links)
            {
                canvas.ReDrawLink(link, calculator);
            }
#endif
        }

        public void OnStarPressed(Star star)
        {
            var command = Command as Command<Star>;
            if (command == null) return;
            if (command.CanExecute(star)) command.Execute(star);

            //Device.OnPlatform(iOS: Invalidate);
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
            var ctrl = ((GameControl)bindable);
            ctrl.Invalidate();

#if __IOS__
            foreach (var link in ctrl.Links)
            {
                var linkCtrl = new LinkControl(link, ctrl.calculator);
                ctrl.layout.Children.Add(linkCtrl);
            }
#endif
        }

        private static void StarsChanged(BindableObject bindable, IEnumerable<Star> oldValue, IEnumerable<Star> newValue)
        {
            var ctrl = (GameControl) bindable;
            ctrl.layout.Children.Clear();

			//By unknown reasons, NControls size and Xamarin.Forms size are different for the same area of display. 
			var xfCalc = new SizeCalculator((int)ctrl.layout.Width, (int)ctrl.layout.Height);

#if __IOS__
            foreach (var link in ctrl.Links)
            {
                var linkCtrl = new LinkControl(link, ctrl.calculator);
                ctrl.layout.Children.Add(linkCtrl);
            }
#endif
            foreach (var star in newValue)
            {
				var starCtrl = new StarControl(star, ctrl.calculator, ctrl.OnStarPressed);
                var hw = xfCalc.RelToAbsByMinSize(star.HalfWidthRel);
                var left = new Point(xfCalc.XRelToAbs(star.XRel) - hw, xfCalc.YRelToAbs(star.YRel) - hw);
				var size = new Size(hw * 2, hw * 2);
				var rect = new Rectangle(left, size);
                
                AbsoluteLayout.SetLayoutBounds(starCtrl, rect);
                                
                ctrl.layout.Children.Add(starCtrl);
            }
        }
    }



#if __IOS__
    public class LinkControl : NControlView
    {
        private readonly Link link;
        private readonly SizeCalculator calculator;

        public LinkControl(Link link, SizeCalculator calculator)
        {
            this.link = link;
            this.calculator = calculator;
        }

        public override void Draw(ICanvas canvas, Rect rect)
        {
            base.Draw(canvas, rect);
            canvas.ReDrawLink(link, calculator);
        }
    }
#endif
}
