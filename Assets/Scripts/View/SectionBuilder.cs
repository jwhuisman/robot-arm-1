using Assets.Models.WorldData;
using Assets.Scripts.WorldData;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.View
{
    public class SectionBuilder : MonoBehaviour
    {
        // models
        public GameObject assemblyLineModel;
        public GameObject floorModel;
        public GameObject wallModel;
        public GameObject wallExtendModel;
        public GameObject blockModel;

        // start
        public void Start()
        {
            _globals = GameObject.Find("Globals").GetComponent<Globals>();
            _view = GameObject.Find("View");
            _cubes = GameObject.Find("Cubes");
            _world = _globals.world;
            _robotArmData = _world.RobotArm;
            _factory = GameObject.Find("Factory");

            CreateStartSections();

            initialized = true;
        }

        // creation
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

        // generation
        public void GenerateFactory(int sectionId)
        {
            float width = sectionWidthTotal;
            float halfW = width / 2;
            float posX = halfW + (sectionId * width);
            float posY = width;
            float posFloorZ = -width;
            float posWallZ = halfW;

            int amount = 4;

            InstantiateAssemblyLine(sectionId, posX);
            InstantiateFloor(sectionId, posX, posFloorZ, amount);
            InstantiateWall(sectionId, posX, posY, posWallZ, amount);
        }
        public void GenerateBlocks(int stackX)
        {
            Stack<Block> blocks = _world.Stacks.Where(c => c.Id == stackX).SingleOrDefault().Blocks;

            foreach (Block block in blocks)
            {
                InstantiateBlock(stackX, block);
            }
        }

        // instantiation
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
                GameObject wall = i == 0 ? Instantiate(wallModel) : Instantiate(wallExtendModel);
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

        // check for generating new section
        public void CheckSections()
        {
            SectionCheck check = NeedNewSection();
            if (initialized && check.NeedNew)
            {
                int newSectionId = check.Section + check.Dir;

                if (!instantiatedSections.Contains(newSectionId))
                {
                    CreateSection(newSectionId);
                }
            }
        }
        public SectionCheck NeedNewSection()
        {
            int min = instantiatedSections.Min(sId => sId);
            int max = instantiatedSections.Max(sId => sId);

            GameObject minSection = GameObject.Find("Assembly-(" + (min) + ")");
            GameObject maxSection = GameObject.Find("Assembly-(" + (max) + ")");

            float minX = Camera.main.WorldToViewportPoint(minSection.transform.position).x;
            float maxX = Camera.main.WorldToViewportPoint(maxSection.transform.position).x;

            bool minVisible = minX >= -.5 && minX <= 1.5 ? true : false;
            bool maxVisible = maxX >= -.5 && maxX <= 1.5 ? true : false;

            if (minVisible && maxVisible)
            {
                return new SectionCheck(min, -1, max, 1);
            }
            else if (minVisible)
            {
                return new SectionCheck(min, -1);
            }
            else if (maxVisible)
            {
                return new SectionCheck(max, 1);
            }

            return new SectionCheck();
        }

        // set section size and spacing
        public void SetSectionSize(int widthTotal, int width, float space)
        {
            sectionWidthTotal = widthTotal;
            sectionWidth = width;
            spacing = space;
        }


        // misc
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


        // privates
        private List<int> instantiatedSections = new List<int>();
        private int sectionWidthTotal;
        private int sectionWidth;
        private float spacing;

        private bool initialized = false;

        private GameObject _view;
        private GameObject _cubes;
        private GameObject _factory;
        private GameObject _robotArm;
        private RobotArmData _robotArmData;
        private Globals _globals;
        private World _world;
    }
}
