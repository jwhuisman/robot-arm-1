using System.Linq;
using System.Collections.Generic;
using Assets.Models.World;

public class World
{
    public List<Section> Sections = new List<Section>();
    public RobotArm RobotArm = new RobotArm();

    public int sectionWidthTotal = 15; // amount stacks that can fit on a single section
    public int sectionWidth = 13; // amount stacks you want on a single section
    public int cubes = 25;

    public World()
    {
        for (int i = -3; i <= 3; i++)
        {
            Sections.Add(GenerateSection(i));
        }
    }

    public Section GenerateSection(int sectionId)
    {
        Section section = new Section(sectionId);
        section.Stacks = GenerateStacks(sectionId);

        return section;
    }

    public List<CubeStack> GenerateStacks(int sectionId)
    {
        List<CubeStack> stacks = new List<CubeStack>();

        for (int i = 0; i < sectionWidth; i++)
        {
            CubeStack stack = new CubeStack(i, i);
            stacks.Add(stack);
        }

        stacks = FillStacks(sectionId, stacks);

        return stacks;
    }
    public List<CubeStack> FillStacks(int sectionId, List<CubeStack> stacks)
    {
        System.Random rnd = new System.Random();

        for (int i = 0; i < cubes; i++)
        {
            int stackId = rnd.Next(0, sectionWidth);

            int x = stacks[stackId].X;
            int y = stacks[stackId].Cubes.Count(); 

            int colorNumber = rnd.Next(0, 4);
            string color = ((ColorEnum.Colors)colorNumber).ToString();
            string id = "(" + sectionId + "/" + i + ")";

            stacks[stackId].Cubes.Push(new Cube(id, color, x, y));
        }

        return stacks;
    }
}