using System.Collections.Generic;

public class CubeStack
{
    public CubeStack()
    {
        cubes = new Stack<Cube>();
    }

    public Stack<Cube> cubes { get;set; }
    public float id;
    public float x;
}