using Assets.Models.World;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class View : MonoBehaviour
    {
        // blender models
        public GameObject assemblyLineModel;
        public GameObject floorModel;
        public GameObject wallModel;
        public GameObject blockModel;
        public GameObject speedMeterModel;
        public GameObject robotArmModel;


        public void Start()
        {
            _globals  = GameObject.Find("Globals").GetComponent<Globals>();
            _view     = GameObject.Find("View");
            _factory  = GameObject.Find("Factory");
            _cubes    = GameObject.Find("Cubes");
            _world    = _globals.world;
            _robotArmData = _world.RobotArm;

            sectionWidthTotal = _world.sectionWidthTotal;
            sectionWidth = _world.sectionWidth;
            spacing = sectionWidthTotal / sectionWidth;

            _robotArm = CreateRobotArm(_robotArmData);
            CreateWorld(_world);
        }

        public void Update()
        {
            _robotArm.transform.position = RobotArmToView(_robotArmData);
        }

        public Vector3 RobotArmToView(RobotArm arm)
        {
            float x = arm.X * spacing;
            float y = arm.Y;

            return new Vector3(x, y);
        }

        public GameObject CreateRobotArm(RobotArm robotArm)
        {
            GameObject arm = Instantiate(robotArmModel);

            arm.name = "Robot Arm";
            arm.tag  = "RobotArm";
            arm.transform.parent = _view.transform;
            arm.transform.position = new Vector3(robotArm.X, robotArm.Y, 0);

            arm.AddComponent<BoxCollider>(); // dont need this in the future

            return arm;
        }
        public void CreateSpeedMeter(Transform parent)
        {
            GameObject arm = Instantiate(speedMeterModel);

            arm.name = "SpeedMeter";
            arm.tag = "SpeedMeter";
            arm.transform.parent = parent;
            arm.transform.position = new Vector3(parent.position.x, parent.position.y - 5f, 0);

            arm.AddComponent<BoxCollider>(); // dont need this in the future
        }
        public void CreateWorld(World world)
        {
            foreach (Section section in world.Sections)
            {
                int id = section.Id;

                GenerateFactory(id);
                GenerateBlocks(id);
            }
        }

        public void DrawWorld(World world)
        {
            foreach (Section section in world.Sections)
            {
                int id = section.Id;

                //DrawBlocks(id);
            }
        }

        public void GenerateFactory(int sectionId)
        {
            float width = 15f;
            float halfW = width / 2;
            float posX = halfW + (sectionId * width);
            float posY = width;
            float posFloorZ = -width;
            float posWallZ = halfW;

            GenerateAssemblyLine(sectionId, posX);
            GenerateFloor(sectionId, 2, posX, posFloorZ);
            GenerateWall(sectionId, 2, posX, posY, posWallZ);
        }

        public void GenerateAssemblyLine(int sectionId, float x)
        {
            GameObject assembly = Instantiate(assemblyLineModel);
            assembly.transform.parent = _factory.transform;
            assembly.name = "assembly-(" + sectionId + ")";
            assembly.transform.position = new Vector3(x - .5f, -.5f);
        }
        public void GenerateFloor(int sectionId, int amount, float x, float z)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject floor = Instantiate(floorModel);
                floor.transform.parent = _factory.transform;
                floor.name = "floor-(" + sectionId + "/" + i + ")";
                floor.transform.position = new Vector3(x - .5f, -.5f, z * (i + 1));
            }
        }
        public void GenerateWall(int sectionId, int amount, float x, float y, float z)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject wall = Instantiate(wallModel);
                wall.transform.parent = _factory.transform;
                wall.name = "wall-(" + sectionId + "/" + i + ")";
                wall.transform.position = new Vector3(x - .5f, (y * i) - .5f, z);
            }
        }

        public void GenerateBlocks(int sectionId)
        {
            foreach (CubeStack cubeStack in _world.Sections.Where(s => s.Id == sectionId).SingleOrDefault().Stacks)
            {
                foreach (Cube cube in cubeStack.Cubes)
                {
                    GenerateBlock(sectionId, cubeStack.X, cube);
                }
            }
        }

        public GameObject GenerateBlock(int sectionId, int stackX, Cube cube)
        {
            GameObject block = Instantiate(blockModel);

            float x = (sectionId * sectionWidthTotal) + (spacing * stackX);

            block.AddComponent<BoxCollider>(); // dont need this in the future

            block.tag = "Cube";
            block.name = "cube-" + cube.Id;
            block.transform.parent = _cubes.transform;
            block.transform.position = new Vector3(x, cube.Y, 0);

            Renderer renderer = block.GetComponent<Renderer>();
            renderer.materials = SetColors(renderer.materials, cube.Color);

            return block;
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


        private float sectionWidthTotal = 15f; // stacks that fit on a section
        private float sectionWidth; // stacks you want on a section
        private float spacing;

        private GameObject _view;
        private GameObject _cubes;
        private GameObject _factory;
        private GameObject _robotArm;
        private RobotArm _robotArmData;
        private Globals _globals;
        private World _world;
    }
}
