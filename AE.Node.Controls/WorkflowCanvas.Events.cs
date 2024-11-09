using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AE.Node.Controls
{
    public partial class WorkflowCanvas
    {
        private Point? dragCanvasStart;
        private Point canvasPosition;
        private Point? dragSelectStart;

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ClearFocus(e);

                if (Keyboard.GetKeyStates(Key.LeftCtrl).HasFlag(KeyStates.Down))
                {
                    Mouse.OverrideCursor = Cursors.SizeAll;

                    dragCanvasStart = e.GetPosition(this);
                    canvasPosition = new Point(translateTransform.X, translateTransform.Y);

                    Canvas.CaptureMouse();
                }
                else
                {
                    dragSelectStart = e.GetPosition(Canvas);

                    SelectBox.Width = 0;
                    SelectBox.Height = 0;

                    SetLeft(SelectBox, dragSelectStart.Value.X - translateTransform.X);
                    SetTop(SelectBox, dragSelectStart.Value.Y - translateTransform.Y);

                    SelectBox.Visibility = Visibility.Visible;
                    SelectBox.CaptureMouse();
                }
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (dragCanvasStart != null)
            {
                e.Handled = true;

                var end = e.GetPosition(this);

                CalculateTranslateTransform(
                    Math.Round(canvasPosition.X + end.X - dragCanvasStart.Value.X),
                    Math.Round(canvasPosition.Y + end.Y - dragCanvasStart.Value.Y)
                );
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.OverrideCursor = null;

            if (dragCanvasStart != null)
            {
                ClearFocus(e);

                Canvas.ReleaseMouseCapture();
                dragCanvasStart = null;
            }
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            if (e.Delta < 0)
            {
                var newScale = Math.Max(MinZoom, Math.Round(scaleTransform.ScaleX - 0.1, 1));

                if (newScale != scaleTransform.ScaleX)
                {
                    var point = e.GetPosition(Canvas);

                    scaleTransform.ScaleX = newScale;
                    scaleTransform.ScaleY = newScale;

                    CalculateTranslateTransform(
                        Math.Round(translateTransform.X + point.X * 0.1),
                        Math.Round(translateTransform.Y + point.Y * 0.1)
                    );
                }
            }
            else if (e.Delta > 0)
            {
                var newScale = Math.Min(MaxZoom, Math.Round(scaleTransform.ScaleX + 0.1, 1));

                if (newScale != scaleTransform.ScaleX)
                {
                    var point = e.GetPosition(Canvas);

                    scaleTransform.ScaleX = newScale;
                    scaleTransform.ScaleY = newScale;

                    CalculateTranslateTransform(
                        Math.Round(translateTransform.X - point.X * 0.1),
                        Math.Round(translateTransform.Y - point.Y * 0.1)
                    );
                }
            }
        }

        private void OnSelectBoxMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && dragSelectStart != null)
            {
                e.Handled = true;

                var end = e.GetPosition(Canvas);

                if (end.X > dragSelectStart.Value.X)
                {
                    SetLeft(SelectBox, dragSelectStart.Value.X);
                    SelectBox.Width = end.X - dragSelectStart.Value.X;
                }
                else if (end.X < dragSelectStart.Value.X)
                {
                    SetLeft(SelectBox, end.X);
                    SelectBox.Width = dragSelectStart.Value.X - end.X;
                }

                if (end.Y > dragSelectStart.Value.Y)
                {
                    SetTop(SelectBox, dragSelectStart.Value.Y);
                    SelectBox.Height = end.Y - dragSelectStart.Value.Y;
                }
                else if (end.Y < dragSelectStart.Value.Y)
                {
                    SetTop(SelectBox, end.Y);
                    SelectBox.Height = dragSelectStart.Value.Y - end.Y;
                }
            }
        }

        private void OnSelectBoxMouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.OverrideCursor = null;

            if (dragSelectStart != null)
            {
                ClearFocus(e);

                var rect = new Rect(
                    GetLeft(SelectBox),
                    GetTop(SelectBox),
                    SelectBox.Width,
                    SelectBox.Height
                );

                foreach (var node in Nodes)
                {
                    if (rect.IntersectsWith(new Rect(node.X, node.Y, node.Width, node.Height)))
                        node.ViewModel.IsSelected = true;
                    else
                        node.ViewModel.IsSelected = false;
                }

                //RaiseSelectedChanged();

                SelectBox.Visibility = Visibility.Collapsed;

                SelectBox.ReleaseMouseCapture();
                dragSelectStart = null;
            }
        }
    }
}
