using AE.Node.Items;

namespace AE.Node.Common
{
    public class BaseWorkflow
    {
        public NodeItem[] Nodes { get; set; }
        public LineItem[] Lines { get; set; }
        public GroupItem[] Groups { get; set; }
    }
}
