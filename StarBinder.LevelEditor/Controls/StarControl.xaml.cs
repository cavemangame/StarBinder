using System.Windows;
using System.Windows.Media;

namespace StarBinder.LevelEditor.Controls
{
    public partial class StarControl
    {
        public StarControl()
        {
            InitializeComponent();
            Root.DataContext = this;
        }

        public static readonly DependencyProperty BackgroundBrushProperty = DependencyProperty.Register(
            "BackgroundBrush", typeof (Brush), typeof (StarControl), new PropertyMetadata(Brushes.Black));

        public Brush BackgroundBrush
        {
            get { return (Brush) GetValue(BackgroundBrushProperty); }
            set { SetValue(BackgroundBrushProperty, value); }
        }

        public static readonly DependencyProperty ForegroundBrushProperty = DependencyProperty.Register(
            "ForegroundBrush", typeof (Brush), typeof (StarControl), new PropertyMetadata(default(Brush)));

        public Brush ForegroundBrush
        {
            get { return (Brush) GetValue(ForegroundBrushProperty); }
            set { SetValue(ForegroundBrushProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof(double), typeof(StarControl), new PropertyMetadata(1.0));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
    }
}
