using UnityEngine;
using System.Linq;
using Assets.Models;
using System.Collections.Generic;
using Assets.Models.Commands;

public class World : MonoBehaviour
{
    // models
    public GameObject blockModel;

    public RobotArmController _robotArmController;
    public int cubes; // cubes each section?


    public void Start()
    {
        _world = GameObject.Find("World");
        _cubeList = GameObject.Find("Cubes");
        _robotArm = GameObject.Find("Robot Arm");

        spacing = 15f / 13f;
        //_world.GetComponent<MoveCommand>().MoveLength = spacing; ???

        for (int i = -2; i <= 2; i++)
        {
            GenerateSection(i);
        }
    }

    public void GenerateSection(int x)
    {
        Section section = new Section(x);

        section.Stacks = GenerateStacks();
        section = GenerateBlocks(section);
    }

    public List<CubeStack> GenerateStacks()
    {
        List<CubeStack> stacks = new List<CubeStack>();

        for (int i = 0; i < sectionWidth; i++)
        {
            CubeStack stack = new CubeStack(i, (i * spacing));
            stacks.Add(stack);
        }

        return stacks;
    }
    public Section GenerateBlocks(Section section)
    {
        System.Random rnd = new System.Random();
        for (int i = 0; i < cubes; i++)
        {
            int stackId = rnd.Next(0, sectionWidth);

            CubeStack stack = section.Stacks.Where(s => s.Id == stackId).SingleOrDefault();
            float x = (section.Id * sectionWidthTotal) + (stack.X);
            float y = stack.Cubes.Count(); 

            int colorNumber = rnd.Next(0, 4);
            string color = ((ColorEnum.Colors)colorNumber).ToString();
            string id = "(" + section.Id + "/" + i + ")";

            GameObject cube = GenerateBlock(id, color, x, y);
            stack.Cubes.Push(cube);
        }

        return section;
    }
    public GameObject GenerateBlock(string id, string color, float x, float y)
    {
        GameObject cube = Instantiate(blockModel);

        cube.AddComponent<BoxCollider>(); // dont need this in the future

        cube.tag = "Cube";
        cube.name = "cube-" + id;
        cube.transform.parent = _cubeList.transform;
        cube.transform.position = new Vector3(x, y, 0);

        Renderer renderer = cube.GetComponent<Renderer>();
        renderer.materials = SetColors(renderer.materials, color);

        return cube;
    }

    public Material[] SetColors(Material[] originals, string color)
    {
        Material[] m = new Material[2];
        m[0] = new Material(originals[0]);
        m[1] = new Material(originals[1]);

        switch (color)
        {
            case "Red":
                m[0].color = Color.red;
                m[1].color = Color.red;
                break;
            case "Green":
                m[0].color = Color.green;
                m[1].color = Color.green;
                break;
            case "Blue":
                m[0].color = Color.blue;
                m[1].color = Color.blue;
                break;
            case "White":
                m[0].color = Color.white;
                m[1].color = Color.white;
                break;
            default:
                m[0].color = Color.white;
                m[1].color = Color.white;
                break;
        }

        return m;
    }


    private int sectionWidthTotal = 15; // amount stacks that can fit on a single section
    private int sectionWidth = 13; // amount stacks you want on a single section
    private float spacing;

    private GameObject _world;
    private GameObject _robotArm;
    private GameObject _cubeList;
}