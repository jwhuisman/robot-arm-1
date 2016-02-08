using Assets.Models.World;
using System;
using System.Collections.Generic;
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


        // start
        public void Start()
        {
            InitObjects();
            InitSectionSize();

            InitRobotArm();

            CreateStartSections();

            initialized = true;
        }

        // update
        public void Update()
        {
            UpdateWorld();

            CheckSections();
        }
        public void UpdateWorld()
        {
            _world = _globals.world;

            _robotArm.transform.position = RobotArmToView(_robotArmData);


            if (_robotArmData.Holding)
            {
                FindBlock(_robotArmData.HoldingBlock.Id).transform.position = new Vector3(_robotArm.transform.position.x, _robotArm.transform.position.y - .6f);
                wasHolding = true;
            }
            else if (wasHolding)
            {
                int i = _world.Stacks.FindIndex(s => s.Id == _robotArmData.X);
                float y = _world.Stacks[i].Blocks.Count() - 1;

                FindBlock(_robotArmData.HoldingBlock.Id).transform.position = new Vector3(_robotArm.transform.position.x, y);

                wasHolding = false;
            }
        }

        // new section
        public void CheckSections()
        {
            SectionCheck s = NeedNewSection();
            if (initialized && s.NeedNew)
            {
                int newSectionId = s.Section + s.Dir;

                if (!instantiatedSections.Contains(newSectionId))
                {
                    CreateSection(newSectionId);
                }
            }
        }
        public SectionCheck NeedNewSection()
        {
            SectionCheck s = new SectionCheck();

            int min = instantiatedSections.Min(sId => sId);
            int max = instantiatedSections.Max(sId => sId);

            int margin = 1; // sections further (you are on 2, it will check if it needs to render 4, not 3)
            int currentSection = GetSectionFromX(_robotArmData.X);
            if (currentSection - margin <= min)
            {
                s.NeedNew = true;
                s.Dir = -1;
                s.Section = currentSection - margin;
            }
            else if (currentSection + margin >= max)
            {
                s.NeedNew = true;
                s.Dir = 1;
                s.Section = currentSection + margin;
            }

            return s;
        }
        public int GetSectionFromX(int x)
        {
            return (RoundToMultiple(x) / sectionWidth) - 1;
        }
        public int RoundToMultiple(int numToRound)
        {
            int multiple = sectionWidth;

            if (multiple == 0)
                return numToRound;

            int remainder = Math.Abs(numToRound) % multiple;
            if (remainder == 0)
                return numToRound;

            if (numToRound < 0)
                return -(Math.Abs(numToRound) - remainder);
            else
                return numToRound + multiple - remainder;
        }

        // initialize
        public void InitObjects()
        {
            _globals = GameObject.Find("Globals").GetComponent<Globals>();
            _view = GameObject.Find("View");
            _factory = GameObject.Find("Factory");
            _cubes = GameObject.Find("Cubes");
            _world = _globals.world;
            _robotArmData = _world.RobotArm;
        }
        public void InitSectionSize()
        {
            sectionWidthTotal = (int)assemblyLineModel.GetComponent<MeshRenderer>().bounds.size.x;
            sectionWidth = sectionWidthTotal - (sectionWidthTotal / 4);
            spacing = (float)sectionWidthTotal / (float)sectionWidth;
        }
        public void InitRobotArm()
        {
            CreateRobotArm(_robotArmData);
            CreateSpeedMeter(_robotArm.transform);
        }

        // create
        public void CreateStartSections()
        {
            for (int i = -3; i < 3; i++)
            {
                CreateSection(i);
            }
        }
        public void CreateSection(int sectionId)
        {
            GenerateFactory(sectionId);

            int startX = sectionId * sectionWidth;
            for (int x = startX; x < startX + sectionWidth; x++)
            {
                InstantiateBlocks(x);
            }

            instantiatedSections.Add(sectionId);
        }
        public void CreateRobotArm(RobotArm robotArm)
        {
            GameObject arm = Instantiate(robotArmModel);

            arm.name = "RobotArm";
            arm.tag  = "RobotArm";
            arm.transform.parent = _view.transform;
            arm.transform.position = new Vector3(robotArm.X, robotArm.Y, 0);
            arm.transform.rotation = Quaternion.identity;

            arm.AddComponent<BoxCollider>(); // dont need this in the future

            _robotArm = arm;
        }
        public void CreateSpeedMeter(Transform parent)
        {
            GameObject meter = Instantiate(speedMeterModel);
            meter.name = "SpeedMeter";
            meter.tag = "SpeedMeter";
            meter.transform.parent = parent;


            // is this considered as a 'hack'? because exporting from blender to unity is a b*tch
            // and resetting all rotations is the only solution i found...
            meter.transform.position = new Vector3(parent.position.x, parent.position.y + 1.5f, 0);
            foreach (Transform child in meter.transform)
            {
                child.rotation = Quaternion.identity;
            }
        }

        // generate
        public void GenerateFactory(int sectionId)
        {
            float width = sectionWidthTotal;
            float halfW = width / 2;
            float posX = halfW + (sectionId * width);
            float posY = width;
            float posFloorZ = -width;
            float posWallZ = halfW;

            InstantiateAssemblyLine(sectionId, posX);
            InstantiateFloor(sectionId, 2, posX, posFloorZ);
            InstantiateWall(sectionId, 2, posX, posY, posWallZ);
        }

        // instantiate
        public void InstantiateAssemblyLine(int sectionId, float x)
        {
            GameObject assembly = Instantiate(assemblyLineModel);
            assembly.transform.parent = _factory.transform;
            assembly.name = "Assembly-(" + sectionId + ")";
            assembly.tag = "Assembly";
            assembly.transform.position = new Vector3(x - .5f, -.5f);
        }
        public void InstantiateFloor(int sectionId, int amount, float x, float z)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject floor = Instantiate(floorModel);
                floor.transform.parent = _factory.transform;
                floor.name = "Floor-(" + sectionId + "/" + i + ")";
                floor.tag = "Floor";
                floor.transform.position = new Vector3(x - .5f, -.5f, z * (i + 1));
            }
        }
        public void InstantiateWall(int sectionId, int amount, float x, float y, float z)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject wall = Instantiate(wallModel);
                wall.transform.parent = _factory.transform;
                wall.name = "Wall-(" + sectionId + "/" + i + ")";
                wall.tag = "Wall";
                wall.transform.position = new Vector3(x - .5f, (y * i) - .5f, z);
            }
        }
        public void InstantiateBlocks(int stackX)
        {
            Stack<Block> cubes = _world.Stacks.Where(c => c.Id == stackX).SingleOrDefault().Blocks;

            foreach (Block cube in cubes)
            {
                InstantiateBlock(stackX, cube);
            }

            instantiatedStacks.Add(stackX);
        }
        public void InstantiateBlock(int stackX, Block blockData)
        {
            GameObject block = Instantiate(blockModel);

            float x = (spacing * (float)stackX);

            block.AddComponent<BoxCollider>(); // dont need this in the future

            block.name = "Block-" + blockData.Id;
            block.tag = "Block";
            block.transform.parent = _cubes.transform;
            block.transform.position = new Vector3(x, blockData.Y, 0);

            Renderer renderer = block.GetComponent<Renderer>();
            renderer.materials = SetColors(renderer.materials, blockData.Color);
        }

        // misc.
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
        public Vector3 RobotArmToView(RobotArm arm)
        {
            float x = arm.X * spacing;
            float y = arm.Y;

            return new Vector3(x, y);
        }
        public GameObject FindBlock(string Id)
        {
            return GameObject.Find("Block-" + Id);
        }


        // privates
        private List<int> instantiatedStacks = new List<int>();
        private List<int> instantiatedSections = new List<int>();

        private int sectionWidthTotal; // stacks that fit on a section
        private int sectionWidth; // stacks you want on a section
        private float spacing;
        private bool wasHolding = false;
        private bool initialized = false;

        private GameObject _view;
        private GameObject _cubes;
        private GameObject _factory;
        private GameObject _robotArm;
        private RobotArm   _robotArmData;
        private Globals    _globals;
        private World      _world;
    }
}