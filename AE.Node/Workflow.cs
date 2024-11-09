using AE.Node.Common;
using AE.Node.Items;

namespace AE.Node
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowConfig
    {
        public string Title { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Workflow : BaseWorkflow
    {
        public int LastId { get; set; }
        public string[] Folders { get; set; }
        public CustomNodeItem[] CustomNodes { get; set; }
        public WorkflowConfig Config { get; set; }
    }
}
