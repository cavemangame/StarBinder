using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Microsoft.Practices.Prism.Mvvm;
using StarBinder.Core;
using StarBinder.Utils;

namespace StarBinder.ViewModels
{
    class StarViewModel : ViewModel
    {
        private readonly SizeCalculator calculator;

        public StarViewModel(Star star, SizeCalculator calculator)
        {
            this.calculator = calculator;
            Model = star;
        }

        public void UpdatePosition()
        {
            OnPropertyChanged("X");
            OnPropertyChanged("Y");
            OnPropertyChanged("XStar");
            OnPropertyChanged("YStar");
            OnPropertyChanged("GeometryBack");
            OnPropertyChanged("GeometryFront");
        }

        public Star Model { get; private set; }

        public int X { get { return calculator.XRelToAbs(Model.XRel); } }
        public int Y { get { return calculator.YRelToAbs(Model.YRel); } }

        //Винфон отказывается хватать события нажатия вне рамок кнопки
        //т.к. центр звезды в 0,0 а рамки кнопки не хотят перестраиваться под контент с отрийательным сдвигом, делаем доп пересчет координат с учетом ширины звезды
        //В WPF и WinApp работает и без этого
        public int XStar { get { return calculator.XRelToAbs(Model.XRel) - calculator.RelToAbsByMinSize(Model.HalfWidthRel); } }
        public int YStar { get { return calculator.YRelToAbs(Model.YRel) - calculator.RelToAbsByMinSize(Model.HalfWidthRel); } }
        public Geometry GeometryBack { get { return calculator.GetWinphonePoints(Model, true).ToPathGeometry(); } }
        public Geometry GeometryFront { get { return calculator.GetWinphonePoints(Model, false).ToPathGeometry(); } }
    }
}
