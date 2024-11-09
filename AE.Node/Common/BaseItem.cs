namespace AE.Node.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseItem
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public double X { get; set; }
        public double Y { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public NodeShapeType Shape { get; set; }
        public string Color { get; set; }
        public bool Enabled { get; set; }
        public bool Pin { get; set; }
    }
}
