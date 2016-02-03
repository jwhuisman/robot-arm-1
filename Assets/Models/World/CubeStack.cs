using System.Collections.Generic;

namespace Assets.Models.World
{
    public class CubeStack
    {
        public CubeStack(int id, int x)
        {
            Cubes = new Stack<Cube>();

            Id = id;
            X = x;
        }

        public Stack<Cube> Cubes { get; set; }
        public int Id { get; set; }
        public int X { get; set; }
    }
}