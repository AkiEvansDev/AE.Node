using AE.Node.Common;

namespace AE.Node.Items
{
    public class CustomNodeItem : BaseWorkflow
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public PinItem[] In { get; set; }
        public PinItem[] Out { get; set; }
    }
}
