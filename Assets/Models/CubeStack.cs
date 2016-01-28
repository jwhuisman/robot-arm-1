using System.Collections.Generic;

public class CubeStack
{
    public CubeStack(int id, float x)
    {
        Id = id;
        X = x;
        Cubes = new Stack<Cube>();
    }

    public Stack<Cube> Cubes { get; set; }
    public int Id { get; set; }
    public float X { get; set; }
}