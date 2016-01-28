using UnityEngine;
using System.Linq;
using Assets.Models;
using System.Collections.Generic;

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

        sectionWidth = (int)(sectionWidthStatic - (sectionWidthStatic * spacing));


        GenerateSection(-1);
        GenerateSection(0);
        GenerateSection(1);
    }

    public void GenerateSection(int x)
    {
        Section section = new Section(x);

        section.Stacks = GenerateStacks(x);
        section = GenerateBlocks(section);
    }

    public List<CubeStack> GenerateStacks(int x)
    {
        List<CubeStack> stacks = new List<CubeStack>();

        for (int i = 0; i < sectionWidth; i++)
        {
            CubeStack stack = new CubeStack(i, i + (i * spacing));
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
            float x = (section.Id * sectionWidthStatic) + (stack.X);
            float y = stack.Cubes.Count(); 

            int colorNumber = rnd.Next(0, 4);
            string color = ((ColorEnum.Colors)colorNumber).ToString();

            GameObject cube = GenerateCube(i, color, x, y);
            stack.Cubes.Push(cube);
        }

        return section;
    }
    public GameObject GenerateCube(int id, string color, float x, float y)
    {
        GameObject cube = Instantiate(blockModel);

        cube.AddComponent<BoxCollider>(); // dont need this in the future
        cube.tag = "Cube";
        cube.name = "cube " + id.ToString();
        cube.transform.parent = _cubeList.transform;
        cube.transform.position = new Vector3(x, y, 0);

        Renderer renderer = cube.GetComponent<Renderer>();
        renderer.materials = SetColors(renderer.materials, color);

        return cube;
    }

    public Material[] SetColors(Material[] original, string color)
    {
        Material[] m = new Material[2];
        m[0] = new Material(original[0]);
        m[1] = new Material(original[1]);

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


    private float sectionWidthStatic = 15; // always the same
    private float spacing = .2f; // space between stacks (keep under 1f)
    private int sectionWidth; // changes depending on the spacing (width - (width * spacing))

    private GameObject _world;
    private GameObject _robotArm;
    private GameObject _cubeList;
}
