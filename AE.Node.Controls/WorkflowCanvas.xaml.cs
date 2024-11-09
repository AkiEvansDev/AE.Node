using AE.Node.Controls.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AE.Node.Controls
{
    public partial class WorkflowCanvas : Canvas
    {
        public const double MaxZoom = 2.2;
        public const double MinZoom = 0.2;

        public const int GroupZIndex = 1;
        public const int LineZIndex = 2;
        public const int NodeZIndex = 3;
        public const int SelectZIndex = 4;

        private readonly TranslateTransform translateTransform;
        private readonly ScaleTransform scaleTransform;

        public WorkflowCanvas()
        {
            InitializeComponent();

            Canvas.LayoutTransform = scaleTransform = new ScaleTransform();
            Canvas.RenderTransform = translateTransform = new TranslateTransform();

            SetZIndex(SelectBox, SelectZIndex);

            var testNode = WorkflowManager.CreateNode(1, "TEST");
            var testNode2 = WorkflowManager.CreateNode(2, "TEST");

            AddNode(testNode, 20, 20);
            AddNode(testNode2, 250, 40);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (sizeInfo.WidthChanged || sizeInfo.HeightChanged)
                CalculateTranslateTransform(translateTransform.X, translateTransform.Y);
        }

        private void CalculateTranslateTransform(double newX, double newY)
        {
            var borderX = Math.Round(ActualWidth - Canvas.Width * scaleTransform.ScaleX);
            var borderY = Math.Round(ActualHeight - Canvas.Height * scaleTransform.ScaleY);

            if (newX < borderX)
                newX = borderX;
            else if (newX > 0)
                newX = 0;

            if (newY < borderY)
                newY = borderY;
            else if (newY > 0)
                newY = 0;

            translateTransform.X = newX;
            translateTransform.Y = newY;
        }

        private static void ClearFocus(RoutedEventArgs e)
        {
            e.Handled = true;
            Keyboard.ClearFocus();
        }
    }
}
