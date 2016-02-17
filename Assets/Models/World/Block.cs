
public class Block
{
    public Block()
    {

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