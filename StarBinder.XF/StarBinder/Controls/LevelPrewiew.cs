using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NControl.Abstractions;
using NGraphics;
using StarBinder.Core;
using Xamarin.Forms;
using Point = NGraphics.Point;

namespace StarBinder.Controls
{
    public class LevelPrewiew : NControlView
    {
        private readonly SizeCalculator calculator = new SizeCalculator();
        
        public LevelPrewiew()
        {
        }

        public override void Draw(ICanvas canvas, Rect rect)
        {
            base.Draw(canvas, rect);

            if (Level == null) return;
            calculator.Resize(rect.Width, rect.Height);

            foreach (var link in Level.Links)
            {
                canvas.DrawLink(link, calculator, 1);
            }

            foreach (var star in Level.Stars)
            {
                canvas.DrawStar(star, calculator, true);
            }
        }

        public static BindableProperty CommandProperty = BindableProperty.Create<LevelPrewiew, Command<Galaxy>>(p => p.Command, default(Command<Galaxy>));
        public Command<Galaxy> Command
        {
            get { return (Command<Galaxy>)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static BindableProperty LevelProperty = BindableProperty.Create<LevelPrewiew, Galaxy>(
            p => p.Level, default(Galaxy), BindingMode.OneWay, null, LevelChanged);

        public Galaxy Level
        {
            get { return (Galaxy)GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }

        private static void LevelChanged(BindableObject bindable, Galaxy oldValue, Galaxy newValue)
        {
            var level = (LevelPrewiew)bindable;
            level.Invalidate();
        }

        public override bool TouchesBegan(IEnumerable<Point> points)
        {
            this.ScaleTo(0.8, 65, Easing.CubicInOut);
            return true;
        }

        public override bool TouchesCancelled(IEnumerable<Point> points)
        {
            this.ScaleTo(1, 65, Easing.CubicInOut);
            return true;
        }

        public override bool TouchesEnded(IEnumerable<Point> points)
        {
            this.ScaleTo(1.2, 65, Easing.CubicInOut);

            if (Command != null && Command.CanExecute(Level)) 
                Command.Execute(Level);
            
            return true;
        }
    }
}
