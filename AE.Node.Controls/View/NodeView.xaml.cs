using AE.Node.Controls.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace AE.Node.Controls.View
{
    public partial class NodeView : Border
    {
        public NodeViewModel ViewModel => DataContext as NodeViewModel;

        public double X
        {
            get => Canvas.GetLeft(this);
            set => Canvas.SetLeft(this, value);
        }

        public double Y
        {
            get => Canvas.GetTop(this);
            set => Canvas.SetTop(this, value);
        }

        public NodeView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (double.IsNaN(Width) || (Width - 2) % 20 != 0)
                Width = Math.Ceiling(RenderSize.Width - (RenderSize.Width % 20) + 22);

            if (double.IsNaN(Height) || (Height - 2) % 20 != 0)
                Height = Math.Ceiling(RenderSize.Height - (RenderSize.Height % 20) + 22);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (IsLoaded)
            {
                if (sizeInfo.WidthChanged && (double.IsNaN(Width) || (Width - 2) % 20 != 0))
                    Width = Math.Ceiling(sizeInfo.NewSize.Width - (sizeInfo.NewSize.Width % 20) + 22);

                if (sizeInfo.HeightChanged && (double.IsNaN(Height) || (Height - 2) % 20 != 0))
                    Height = Math.Ceiling(sizeInfo.NewSize.Height - (sizeInfo.NewSize.Height % 20) + 22);
            }
        }
    }
}
