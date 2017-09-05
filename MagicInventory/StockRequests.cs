namespace MagicInventory
{
    public class StockRequests
    {
        public int Id { get; set; }
        public string Store { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int StockLevel { get; set; }
        public bool Available { get; set; }
    }
}
