using System.Collections.Generic;
using UnityEngine;

public class CubeStack
{
    public CubeStack(int id, float x)
    {
        Cubes = new Stack<GameObject>();

        Id = id;
        X = x;
    }

    public Stack<GameObject> Cubes { get; set; }
    public int Id { get; set; }
    public float X { get; set; }
}