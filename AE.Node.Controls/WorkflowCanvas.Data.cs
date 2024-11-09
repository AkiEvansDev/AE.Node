using AE.Node.Controls.View;
using AE.Node.Controls.ViewModels;
using AE.Node.Items;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AE.Node.Controls
{
    public partial class WorkflowCanvas
    {
        private void AddNode(NodeItem nodeItem, double x, double y)
        {
            var node = new NodeView
            {
                DataContext = new NodeViewModel(nodeItem),
                X = x,
                Y = y,
            };

            SetZIndex(node, NodeZIndex);
            NormalizeNodePosition(node);

            node.MouseDown += OnNodeMouseDown;
            node.MouseMove += OnNodeMouseMove;
            node.MouseUp += OnNodeMouseUp;

            Canvas.Children.Add(node);
        }
    }
}
