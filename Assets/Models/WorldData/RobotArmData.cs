using Assets.Models.WorldData;

public class RobotArmData
{
    public RobotArmData(int y)
    {
        HoldingBlock = new Block();
        Holding = false;
        X = 0;
        Y = y;
    }

    public Block HoldingBlock { get; set; }
    public bool Holding { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}