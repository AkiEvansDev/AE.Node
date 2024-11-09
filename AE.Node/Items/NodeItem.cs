using AE.Node.Common;

namespace AE.Node.Items
{
    /// <summary>
    /// 
    /// </summary>
    public class NodeItem : BaseItem
    {
        public string Type { get; set; }

        public PinItem[] In { get; set; }
        public PinItem[] Out { get; set; }
        public PinValue[] Values { get; set; }

        public bool Collapse { get; set; }
    }
}
