using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class World : MonoBehaviour {
    
    public int size;
    public int cubes;

	public void Start()
    {
        float x = -(size / 2);
        
        for (float i = -(size / 2); i < (size / +1); i += 1f)
        {
            CubeStack cubeStack = new CubeStack();
            cubeStack.x = x;
            cubeStack.id = i;
            x = x + 1.2f;
            _cubes.stack.Add(cubeStack);
        }

        GenerateAssembly();
    }

    public void GenerateAssembly()
    {
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
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Renderer rend = cube.GetComponent<Renderer>();
            switch(block.color)
            {
                case "Red":
                    rend.material.color = Color.red;
                    break;
                case "Green":
                    rend.material.color = Color.green;
                    break;
                case "Blue":
                    rend.material.color = Color.blue;
                    break;
                case "White":
                    rend.material.color = Color.white;
                    break;
                default:
                    rend.material.color = Color.white;
                    break;
            }
            Vector3 xyz = new Vector3(stack.x, y, 0);
            cube.transform.position = xyz;
        }
    }

    private RobotArm _robotArm;
    private CubeStack _cubeStack;
    private Cube _cube;
    public Cubes _cubes = new Cubes();
}
