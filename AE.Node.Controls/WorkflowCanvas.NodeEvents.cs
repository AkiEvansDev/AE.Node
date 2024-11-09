using AE.Node.Controls.View;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AE.Node.Controls
{
    public partial class WorkflowCanvas
    {
        private Point? dragNodeStart;
        private Point? saveNodePos;

        private IEnumerable<NodeView> Nodes => Canvas.Children.OfType<NodeView>();
        private IEnumerable<NodeView> SelectedNodes => Nodes.Where(n => n.ViewModel.IsSelected);

        private void OnNodeMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is NodeView node)
            {
                if (!node.ViewModel.Pin && e.LeftButton == MouseButtonState.Pressed)
                {
                    ClearFocus(e);

                    Mouse.OverrideCursor = Cursors.SizeAll;

                    dragNodeStart = e.GetPosition(node);
                    saveNodePos = new Point(node.X, node.Y);

                    node.CaptureMouse();
                }
                else if (e.RightButton == MouseButtonState.Pressed)
                {
                    ClearFocus(e);

                    node.ViewModel.IsSelected = !node.ViewModel.IsSelected;
                }
            }
        }

        private void OnNodeMouseMove(object sender, MouseEventArgs e)
        {
            if (dragNodeStart != null && e.LeftButton == MouseButtonState.Pressed && sender is NodeView node)
            {
                e.Handled = true;

                var end = e.GetPosition(Canvas);

                var newX = end.X - dragNodeStart.Value.X;
                var newY = end.Y - dragNodeStart.Value.Y;

                foreach (var subNode in SelectedNodes)
                {
                    if (ReferenceEquals(node, subNode))
                        continue;

                    MoveNode(subNode, subNode.X + newX - node.X, subNode.Y + newY - node.Y);
                }

                MoveNode(node, newX, newY);
            }
        }

        private void MoveNode(NodeView node, double newX, double newY)
        {
            if (newX < 20)
                newX = 19;

            if (newY < 20)
                newY = 19;

            if (node.Width + newX > Canvas.Width - 19)
                newX = Canvas.Width - 19 - node.Width;

            if (node.Height + newY > Canvas.Height - 19)
                newY = Canvas.Height - 19 - node.Height;

            if (newX != 0)
                node.X = newX;

            if (newY != 0)
                node.Y = newY;
        }

        private void OnNodeMouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.OverrideCursor = null;

            if (dragNodeStart != null && sender is NodeView node)
            {
                ClearFocus(e);

                foreach (var subNode in SelectedNodes)
                {
                    if (ReferenceEquals(node, subNode))
                        continue;

                    NormalizeNodePosition(subNode);
                }

                NormalizeNodePosition(node);

                //var history = new HistoryMoveNode(node.Data.Item.Id)
                //{
                //    ChangeX = node.X - saveNodePos.Value.X,
                //    ChangeY = node.Y - saveNodePos.Value.Y,
                //};

                //if (node.ViewModel.IsSelected)
                //    foreach (var subNode in Canvas.Children.OfType<NodeView>().Where(n => n.ViewModel.IsSelected && !ReferenceEquals(node, n)))
                //    {
                //        if (subNode.X % 20 < 10)
                //            subNode.X = Math.Ceiling(subNode.X - (subNode.X % 20) - 1);
                //        else
                //            subNode.X = Math.Ceiling(subNode.X - (subNode.X % 20) + 19);

                //        if (subNode.Y % 20 < 10)
                //            subNode.Y = Math.Ceiling(subNode.Y - (subNode.Y % 20) - 1);
                //        else
                //            subNode.Y = Math.Ceiling(subNode.Y - (subNode.Y % 20) + 19);

                //        //history.ItemIds.Add(subNode.Data.Item.Id);
                //    }

                node.ReleaseMouseCapture();
                dragNodeStart = null;
                saveNodePos = null;

                //HistoryHelper.Push(Data.Name, history);
            }
        }

        private static void NormalizeNodePosition(NodeView node)
        {
            if (node.X % 20 < 10)
                node.X = Math.Ceiling(node.X - (node.X % 20) - 1);
            else
                node.X = Math.Ceiling(node.X - (node.X % 20) + 19);

            if (node.Y % 20 < 10)
                node.Y = Math.Ceiling(node.Y - (node.Y % 20) - 1);
            else
                node.Y = Math.Ceiling(node.Y - (node.Y % 20) + 19);
        }
    }
}
