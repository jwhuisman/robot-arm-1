using System.Linq;
using System.Collections.Generic;
using Assets.Models.World;

public class World
{
    public List<CubeStack> Stacks = new List<CubeStack>();
    public RobotArm RobotArm = new RobotArm();
   

    public World()
    {
        for (int x = stackMin; x < stackMax; x++)
        {
            AddStack(x);
        }
    }

    public void AddStack(int x)
    {
        CubeStack stack = new CubeStack(x);

        int cubesAmount = rnd.Next(minCubes, maxCubes + 1);

        for (int i = 0; i < cubesAmount; i++)
        {
            int y = stack.Cubes.Count(); 

            int colorNumber = rnd.Next(0, 4);
            string color = ((ColorEnum.Colors)colorNumber).ToString();
            string id = "(" + x + "/" + i + ")";

            stack.Cubes.Push(new Cube(id, color, x, y));
        }

        Stacks.Add(stack);
    }


    private System.Random rnd = new System.Random();
    private int stackMin = -1000;
    private int stackMax = 1000;
    private int minCubes = 1;
    private int maxCubes = 6;
}