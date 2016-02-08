﻿using Assets.Models.World;

public class RobotArm
{
    public RobotArm(int y)
    {
        Holding = false;
        X = 0;
        Y = y;
    }

    public Block HoldingBlock { get; set; }
    public bool Holding { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}