using System.Collections.Generic;

public class CubeStack
{
    public CubeStack()
    {
        cubes = new Stack<Cube>();
    }
    public float id;
    public float x;
    public Stack<Cube> cubes { get;set; }
}
