using AE.Node.Items;

namespace AE.Node.Interface
{
    public interface INodeType
    {
        public string DefaultTitle { get; }
        public string Name { get; }
        public string Group { get; }

        public PinItem[] In { get; }
        public PinItem[] Out { get; }
        public PinValue[] Values { get; }

        public string DefaultColor { get; }
        public INodeBuilder Builder { get; }
    }

    public interface INodeBuilder
    {

    }
}
