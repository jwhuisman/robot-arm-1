using UnityEngine;
using System.Linq;

public class World : MonoBehaviour
{
    public RobotArmController _robotArmController;
    public GameObject industrial_block;
    public int size;
    public int cubes;

    public void Start()
    {
        float x = -(size / 2);
        float midX = 0;
        
        for (float i = -(size / 2); i < (size /2 +1); i += 1f)
        {
            CubeStack cubeStack = new CubeStack();
            cubeStack.x = x;
            cubeStack.id = i;
            _cubes.stack.Add(cubeStack);
            if (i == 0)
            {
                midX = x;
            }
            
            x = x + 1.2f;
        }
        
        GameObject Plane = GameObject.Find("Plane");
        GameObject Cam = GameObject.Find("Main Camera");
        GameObject RobotArm = GameObject.Find("Robot Arm");
        RobotArm.transform.position = new Vector3(midX, RobotArm.transform.position.y, RobotArm.transform.position.z);
        Plane.transform.position = new Vector3(midX, Plane.transform.position.y, Plane.transform.position.z);
        Plane.transform.localScale = new Vector3((size / 10 + (size/50*1.2f)), Plane.transform.localScale.y, Plane.transform.localScale.z);
        Cam.transform.position = new Vector3( RobotArm.transform.position.x, Cam.transform.position.y, Cam.transform.position.z);

        GenerateAssembly();
    }

    public void GenerateAssembly()
    {
        GameObject CubeList = GameObject.Find("Cubes");
        System.Random rnd = new System.Random();
        for(float i = 0; i < cubes; i += 1f)
        {
            int num = rnd.Next(-(size/2), (size/2+1));
            CubeStack stack = _cubes.stack.Where(l => l.id == num).SingleOrDefault();
            float y = stack.cubes.Count();
            int colorNumber = rnd.Next(0, 4);
            Cube block = new Cube();
            block.color = ((ColorEnum.Colors)colorNumber).ToString();
            stack.cubes.Push(block);
            GameObject cube = Instantiate(industrial_block);

            cube.AddComponent<BoxCollider>();

            cube.tag = "Cube";
            cube.name = "cube " + i.ToString();
            cube.transform.parent = CubeList.transform;
            Renderer renderer = cube.GetComponent<Renderer>();

            renderer.materials = SetColors(renderer.materials, block.color);

            cube.transform.position = new Vector3(stack.x, y, 0);
        }
        int stackSize = 0;
        foreach(var stack in _cubes.stack)
        {
           if(stack.cubes.Count > stackSize)
            {
                stackSize = stack.cubes.Count;
            }
        }
        GameObject RobotArm = GameObject.Find("Robot Arm");
        RobotArm.transform.position = new Vector3(RobotArm.transform.position.x, stackSize + 2f, RobotArm.transform.position.z);
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

    private RobotArm _robotArm;
    private CubeStack _cubeStack;
    private Cube _cube;
    private Cubes _cubes = new Cubes();
}
