namespace AE.Node.Items
{
    /// <summary>
    /// 
    /// </summary>
    public class PinItem
    {
        public string Title { get; set; }
        public string Type { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PinValue : PinItem
    {
        public string Value { get; set; }
    }
}
