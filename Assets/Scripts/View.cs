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
            // should update only after a command is fired.
            UpdateWorld();
            CheckSections();
            UpdateBlockUnderneath();
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
                _robotArmData.HoldingBlock = new Block();

                wasHolding = false;
            }
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
                s.Section = currentSection - margin;
                s.Dir = -1;
                s.NeedNew = true;
            }
            else if (currentSection + margin >= max)
            {
                s.Section = currentSection + margin;
                s.Dir = 1;
                s.NeedNew = true;
            }

            return s;
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
                GenerateBlocks(x);
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

            _robotArm = arm;
        }
        public void CreateSpeedMeter(Transform parent)
        {
            GameObject meter = Instantiate(speedMeterModel);
            meter.name = "SpeedMeter";
            meter.tag = "SpeedMeter";
            meter.transform.parent = parent;

            // reset the rotations, to fix issues with blender -> unity
            meter.transform.position = new Vector3(parent.position.x, parent.position.y + 2.5f, 0);
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
            InstantiateFloor(sectionId, posX, posFloorZ, 2);
            InstantiateWall(sectionId, posX, posY, posWallZ, 2);
        }
        public void GenerateBlocks(int stackX)
        {
            Stack<Block> cubes = _world.Stacks.Where(c => c.Id == stackX).SingleOrDefault().Blocks;

            foreach (Block cube in cubes)
            {
                InstantiateBlock(stackX, cube);
            }

            instantiatedStacks.Add(stackX);
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
        public void InstantiateFloor(int sectionId, float x, float z, int amount = 1)
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
        public void InstantiateWall(int sectionId, float x, float y, float z, int amount = 1)
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
        public void InstantiateBlock(int stackX, Block blockData)
        {
            GameObject block = Instantiate(blockModel);

            float x = (spacing * (float)stackX);

            block.name = "Block-" + blockData.Id;
            block.tag = "Block";
            block.transform.parent = _cubes.transform;
            block.transform.position = new Vector3(x, blockData.Y, 0);

            Renderer renderer = block.GetComponent<Renderer>();
            renderer.materials = SetColors(renderer.materials, blockData.Color);
        }

        // helpers
        public GameObject FindBlock(string Id)
        {
            return GameObject.Find("Block-" + Id);
        }
        public GameObject FindBlockAtX(int x)
        {
            GameObject[] stack = GameObject.FindGameObjectsWithTag("Block").Where(b => b.transform.position.x == WorldToView(x)).ToArray();

            if (stack.Count() != 0)
            {
                float  y = stack.Max(s => s.transform.position.y);
                return stack.Where(s => s.transform.position.y == y).SingleOrDefault();
            }

            return null;
        }
        public float WorldToView(int x)
        {
            return x * spacing;
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
                case "selected":
                    m[0].color = SetGlow(oldMaterials[0].color);
                    m[1].color = SetGlow(oldMaterials[1].color);
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


        // keep this??
        public void UpdateBlockUnderneath()
        {
            GameObject block = FindBlockAtX(_robotArmData.X);

            if (block != null)
            {
                SetGlowToBlock(block);
            }
        }
        public void SetGlowToBlock(GameObject block)
        {
            if (lastBlock != null)
            {
                lastBlock.GetComponent<Renderer>().materials = oldMaterials;
                lastBlock = null;
            }

            if (block.name == "Block-" + _robotArmData.HoldingBlock.Id)
            {
                block.GetComponent<Renderer>().materials = oldMaterials;
            }
            else if (block.name != "Block-" + _robotArmData.HoldingBlock.Id)
            {
                oldMaterials = block.GetComponent<Renderer>().materials;
                lastBlock = block;

                block.GetComponent<Renderer>().materials = SetColors(block.GetComponent<Renderer>().materials, "selected");
            }
                
        }
        public Color SetGlow(Color color)
        {
            float d = (float)Math.Sin(Time.time * 7) / 4;
            return new Color(color.r - d, color.g - d, color.b - d);
        }

        // privates
        private List<int> instantiatedStacks = new List<int>();
        private List<int> instantiatedSections = new List<int>();

        private bool initialized = false;
        private bool wasHolding = false;
        private int sectionWidthTotal;
        private int sectionWidth;
        private float spacing;

        private Material[] oldMaterials = new Material[2];
        private GameObject lastBlock;

        private GameObject _view;
        private GameObject _cubes;
        private GameObject _factory;
        private GameObject _robotArm;
        private RobotArm   _robotArmData;
        private Globals    _globals;
        private World      _world;
    }
}