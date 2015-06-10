using Windows.Foundation;
using Windows.UI.Xaml;

namespace StarBinder.Views
{
    public sealed partial class GamePage
    {
        public GamePage()
        {
            this.InitializeComponent();
        }

        private void GameGrid_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetGameBoardSize(this, e.NewSize);
        }

        public static readonly DependencyProperty GameBoardSizeProperty = DependencyProperty.RegisterAttached(
            "GameBoardSize", typeof (Size), typeof (GamePage), new PropertyMetadata(default(Size)));

        public static void SetGameBoardSize(DependencyObject element, Size value)
        {
            element.SetValue(GameBoardSizeProperty, value);
        }

        public static Size GetGameBoardSize(DependencyObject element)
        {
            return (Size) element.GetValue(GameBoardSizeProperty);
        }
    }
}
