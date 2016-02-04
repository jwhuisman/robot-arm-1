public class RobotArm
{
    public RobotArm()
    {
        Holding = false;
        X = 0;
        Y = 0;
    }

    public bool Holding { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}