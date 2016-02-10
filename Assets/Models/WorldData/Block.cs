namespace Assets.Models.WorldData
{
    public class Block
    {
        public Block()
        {
            Id = "";
            Color = "";
            X = 0;
            Y = 0;
        }
        public Block(string id, string color, int x, int y)
        {
            Id = id;
            Color = color;
            X = x;
            Y = y;
        }

        public string Id { get; set; }
        public string Color { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}