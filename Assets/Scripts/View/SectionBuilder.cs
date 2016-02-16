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
            _world = _globals.world;
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
            _currentSection = new GameObject("Section_"+sectionId);
            _currentSection.tag = "Section";
            _currentSection.transform.parent = _factory.transform;
            _currentSection.transform.position = new Vector3((sectionId * sectionWidthTotal) + (sectionWidthTotal / 2), 0);

            _currentBlocks = new GameObject("Blocks");
            _currentBlocks.transform.parent = _currentSection.transform;

            GenerateFactory(sectionId);

            int startX = sectionId * sectionWidth;
            for (int x = startX; x < startX + sectionWidth; x++)
            {
                GenerateBlocks(x);
            }

            instantiatedSections.Add(sectionId);
            _currentSection = null;
        }

        // generation
        public void GenerateFactory(int sectionId)
        {
            float width = sectionWidthTotal;
            float halfW = width / 2;
            float posX = halfW + (sectionId * width);
            float posY = width;
            float posFloorZ = 0;
            float posWallZ = halfW;

            int amountF = 2;
            int amountW = 4;

            InstantiateAssemblyLine(sectionId, posX);
            InstantiateFloor(sectionId, posX, posFloorZ, width, amountF);
            InstantiateWall(sectionId, posX, posY, posWallZ, amountW);
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
            assembly.transform.parent = _currentSection.transform;
            assembly.name = "Assembly";
            assembly.tag = "Assembly";
            assembly.transform.position = new Vector3(x - .5f, -1.5f);
        }
        public void InstantiateFloor(int sectionId, float x, float z, float width, int amount = 1)
        {
            GameObject floorT = new GameObject("Floor");
            floorT.transform.parent = _currentSection.transform;

            for (int i = 0; i < amount; i++)
            {
                GameObject floor = Instantiate(floorModel);
                floor.transform.parent = floorT.transform;
                floor.name = "Floor_" + i;
                floor.tag = "Floor";
                floor.transform.position = new Vector3(x - .5f, -.5f, (i * -width));
            }
        }
        public void InstantiateWall(int sectionId, float x, float y, float z, int amount = 1)
        {
            GameObject wallT = new GameObject("Wall");
            wallT.transform.parent = _currentSection.transform;

            for (int i = 0; i < amount; i++)
            {
                GameObject wall = i == 0 ? Instantiate(wallModel) : Instantiate(wallExtendModel);
                wall.transform.parent = wallT.transform;
                wall.name = "Wall_" + i;
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
            block.transform.parent = _currentBlocks.transform;
            block.transform.position = new Vector3(x, blockData.Y, 0);

            Renderer renderer = block.GetComponent<Renderer>();
            renderer.materials = SetColors(renderer.materials, blockData.Color);
        }

        // check for generating new section
        public void CheckSectionsToCreate()
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

            GameObject minSection = GameObject.Find("Section_" + min);
            GameObject maxSection = GameObject.Find("Section_" + max);

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
        public void CheckSectionsToRender()
        {
            GameObject[] sections = GameObject.FindGameObjectsWithTag("Section");

            foreach (GameObject section in sections)
            {
                float sectionX = Camera.main.WorldToViewportPoint(section.transform.position).x;
                bool inView = sectionX >= -1f && sectionX <= 2f ? true : false;

                if (inView)
                {
                    foreach (Renderer child in section.GetComponentsInChildren<Renderer>())
                    {
                        if (!child.enabled)
                        {
                            child.enabled = true;
                        }
                    }
                }
                else if (!inView)
                {
                    foreach (Renderer child in section.GetComponentsInChildren<Renderer>())
                    {
                        if (child.enabled)
                        {
                            child.enabled = false;
                        }
                    }
                }
            }
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
        private GameObject _currentSection;
        private GameObject _currentBlocks;

        private int sectionWidthTotal;
        private int sectionWidth;
        private float spacing;

        private bool initialized = false;

        private GameObject _factory;
        private Globals _globals;
        private World _world;
    }
}
