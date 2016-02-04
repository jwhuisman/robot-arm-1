using System.Collections.Generic;

namespace Assets.Models.World
{
    public class CubeStack
    {
        public CubeStack(int id)
        {
            Cubes = new Stack<Cube>();
            Id = id;
        }

        public Stack<Cube> Cubes { get; set; }
        public int Id { get; set; }
    }
}