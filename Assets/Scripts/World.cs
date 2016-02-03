using System.Linq;
using System.Collections.Generic;
using Assets.Models.World;

public class World
{
    public List<Section> Sections;

    public int cubes = 25;

    public World()
    {
        Sections = new List<Section>();

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

        stacks = GenerateBlocks(sectionId, stacks);

        return stacks;
    }

    public List<CubeStack> GenerateBlocks(int sectionId, List<CubeStack> stacks)
    {
        System.Random rnd = new System.Random();

        for (int i = 0; i < cubes; i++)
        {
            int stackId = rnd.Next(0, sectionWidth);

            CubeStack stack = stacks[stackId];
            int x = stack.X;
            int y = stack.Cubes.Count(); 

            int colorNumber = rnd.Next(0, 4);
            string color = ((ColorEnum.Colors)colorNumber).ToString();
            string id = "(" + sectionId + "/" + i + ")";

            stacks[stackId].Cubes.Push(new Cube(id, color, x, y));
        }

        return stacks;
    }


    private int sectionWidthTotal = 15; // amount stacks that can fit on a single section
    private int sectionWidth = 13; // amount stacks you want on a single section
}