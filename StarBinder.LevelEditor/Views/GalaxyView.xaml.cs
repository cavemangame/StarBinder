using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Microsoft.Practices.Prism.Mvvm;

namespace StarBinder.LevelEditor.Views
{
    public partial class GalaxyView : IView
    {
        public GalaxyView()
        {
            InitializeComponent();
        }

        private void Thumb_OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            //Debug.WriteLine(e);
        }

        private void UIElement_OnDragEnter(object sender, DragEventArgs e)
        {
            Debug.WriteLine(e);
        }

        private void UIElement_OnDragLeave(object sender, DragEventArgs e)
        {
            Debug.WriteLine(e);
        }

        private void UIElement_OnDragOver(object sender, DragEventArgs e)
        {
            Debug.WriteLine(e);
        }

        private void UIElement_OnDrop(object sender, DragEventArgs e)
        {
            Debug.WriteLine(e);
        }

        private void UIElement_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void UIElement_OnPreviewDragEnter(object sender, DragEventArgs e)
        {
            Debug.WriteLine(e);
        }

        private void UIElement_OnPreviewDragLeave(object sender, DragEventArgs e)
        {
            Debug.WriteLine(e);
        }

        private void UIElement_OnPreviewDrop(object sender, DragEventArgs e)
        {
            Debug.WriteLine(e);
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("Enter");
        }
    }
}
