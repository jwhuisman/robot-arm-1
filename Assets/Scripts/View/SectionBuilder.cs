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

        public GameObject redBlockModel;
        public GameObject greenBlockModel;
        public GameObject blueBlockModel;
        public GameObject whiteBlockModel;

        public GameObject wallModel;
        public GameObject wallExtendModel;
        public GameObject wallInsetModel;
        public GameObject wallInsetExtendModel;
        public GameObject wallInsetLeftModel;
        public GameObject wallInsetLeftExtendModel;
        public GameObject wallInsetRightModel;
        public GameObject wallInsetRightExtendModel;

        // start
        public void Start()
        {
            _pool = GameObject.Find(Tags.BlockPool).GetComponent<BlockPool>();
            _globals = GameObject.Find(Tags.Globals).GetComponent<Globals>();
            _world = _globals.world;
            _factory = GameObject.Find(Tags.Factory);

            rnd = new System.Random();

            CreateStartSections();

            initialized = true;
        }

        // reload after load level
        public void Reload()
        {
            // delete the factory
            Transform factory = _factory.transform;
            foreach (Transform child in factory.GetComponentInChildren<Transform>())
            {
                Destroy(child.gameObject);
            }
            instantiatedSections = new List<Section>();

            // create the first few sections
            CreateStartSections();

            // set robot arm to x = 0
            Transform robotArm = GameObject.Find(Tags.RobotArm).transform;
            robotArm.position = new Vector3(0, 0, robotArm.position.z);

            // check what to render/create
            CheckSectionsToCreate();
        }


        // creation
        public void CreateStartSections(int currentSection = 0)
        {
            for (int i = currentSection - 3; i <= currentSection + 3; i++)
            {
                CreateSection(i, 0);
                CheckWallsToRender(i);
            }
        }
        public void CreateSection(int sectionId, int dir = 0)
        {
            _currentSection = new GameObject("Section_"+sectionId);
            _currentSection.tag = "Section";
            _currentSection.transform.parent = _factory.transform;
            _currentSection.transform.position = new Vector3((sectionId * sectionWidthTotal) + (sectionWidthTotal / 2), 0);

            _currentBlocks = new GameObject("Blocks");
            _currentBlocks.transform.parent = _currentSection.transform;

            int type = GenerateWallType(sectionId, dir);

            GenerateFactory(sectionId, type);

            int startX = sectionId * sectionWidth;
            for (int x = startX; x < startX + sectionWidth; x++)
            {
                GenerateBlocks(x);
            }

            instantiatedSections.Add(new Section(sectionId, type));
            _currentSection = null;
        }

        // generation
        public void GenerateFactory(int sectionId, int type)
        {
            float width = sectionWidthTotal;
            float halfW = width / 2;
            float posX = halfW + (sectionId * width);
            float posY = width;
            float posFloorZ = 0;
            float posWallZ = halfW;

            int amountF = 2;
            int amountW = 2;

            InstantiateAssemblyLine(sectionId, posX);
            InstantiateFloor(sectionId, posX, posFloorZ, width, amountF);
            InstantiateWall(sectionId, posX, posY, posWallZ, type, amountW);
        }
        public void GenerateBlocks(int stackX)
        {
            if (stackX >= -Levels.stackMax && stackX <= Levels.stackMax)
            {
                Stack<Block> blocks = _world.Stacks[Levels.stackMax + stackX].Blocks;
                if (blocks.Count > 0)
                {
                    foreach (Block block in blocks)
                    {
                        InstantiateBlock(stackX, block);
                    }
                }
            }
        }
        public int GenerateWallType(int sectionId, int dir)
        {
            // types:
            // 0 = normal
            // 1 = inset
            // 2 = inset corner left
            // 3 = inset corner right

            if (dir == 0) { return 0; }

            int prevType = instantiatedSections.Where(s => s.Id == sectionId - dir).SingleOrDefault().Type;

            return GetRandom(prevType, dir);
        }
        public int GetRandom(int prevType, int dir)
        {
            int type = rnd.Next(0, 4);

            if (dir == 0)
            {
                type = 0;
            }
            else if (dir == -1)
            {
                if (prevType == 0 && type == 1 || prevType == 0 && type == 2 ||
                    prevType == 1 && type == 3 || prevType == 1 && type == 0 ||
                    prevType == 2 && type == 3 || prevType == 2 && type == 1 || prevType == 2 && type == 2 ||
                    prevType == 3 && type == 0 || prevType == 3 && type == 3)
                {
                    type = GetRandom(prevType, dir);
                }
            }
            else if (dir == 1)
            {
                if (prevType == 0 && type == 1 || prevType == 0 && type == 3 ||
                    prevType == 1 && type == 2 || prevType == 1 && type == 0 ||
                    prevType == 2 && type == 2 || prevType == 2 && type == 0 ||
                    prevType == 3 && type == 1 || prevType == 3 && type == 2 || prevType == 3 && type == 3)
                {
                    type = GetRandom(prevType, dir);
                }
            }

            return type;
        }
        
        // instantiation
        public void InstantiateAssemblyLine(int sectionId, float x)
        {
            GameObject assembly = Instantiate(assemblyLineModel);
            assembly.transform.parent = _currentSection.transform;
            assembly.name = "Assembly";
            assembly.tag = Tags.Assembly;
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
                floor.tag = Tags.Floor;
                floor.transform.position = new Vector3(x - .5f, -.5f, (i * -width));
            }
        }
        public void InstantiateWall(int sectionId, float x, float y, float z, int type, int amount = 1, int offset = 0, bool useOriginalTransform = false)
        {
            GameObject wallT;
            if (!useOriginalTransform)
            {
                wallT = new GameObject("Wall");
                wallT.transform.parent = _currentSection.transform;
            }
            else
            {
                _currentSection = GameObject.Find("Section_" + sectionId);
                wallT = _currentSection.transform.Find(Tags.Wall).gameObject;
                wallT.transform.parent = _currentSection.transform;
            }

            for (int i = offset; i < offset + amount; i++)
            {
                GameObject wall = (type == 1) ? ((i == 0) ? Instantiate(wallInsetModel) : Instantiate(wallInsetExtendModel)) :
                       (type == 2) ? ((i == 0) ? Instantiate(wallInsetLeftModel) : Instantiate(wallInsetLeftExtendModel)) :
                       (type == 3) ? ((i == 0) ? Instantiate(wallInsetRightModel) : Instantiate(wallInsetRightExtendModel)) : 
                       ((i == 0) ? Instantiate(wallModel) : Instantiate(wallExtendModel));

                wall.transform.parent = wallT.transform;
                wall.name = useOriginalTransform ? "Wall_" + type + "_" + (i + 1) : "Wall_" + type + "_" + i;
                wall.tag = Tags.Wall;
                wall.transform.position = useOriginalTransform ? new Vector3(x - .5f, (y * (i+1)) - .5f, z) : new Vector3(x - .5f, (y * i) - .5f, z);
            }
        }
        public void InstantiateBlock(int stackX, Block blockData)
        {
            GameObject block;
            if (blockData.Color == "Red")
            {
                block = _pool.GetObjectForType("block_red");
            }
            else if (blockData.Color == "Green")
            {
                block = _pool.GetObjectForType("block_green");
            }
            else if (blockData.Color == "Blue")
            {
                block = _pool.GetObjectForType("block_blue");
            }
            else
            {
                block = _pool.GetObjectForType("block_white");
            }

            float x = (spacing * (float)stackX);

            //block.name = "Block-" + blockData.Id;
            block.tag = Tags.Block;
            block.transform.parent = _currentBlocks.transform;
            block.transform.position = new Vector3(x, blockData.Y, 0);
        }

        // check for generating/rendering new section
        public SectionCheck NeedNewSection()
        {
            int min = instantiatedSections.Min(s => s.Id);
            int max = instantiatedSections.Max(s => s.Id);

            GameObject minSection = GameObject.Find("Section_" + min);
            GameObject maxSection = GameObject.Find("Section_" + max);

            float minX = Camera.main.WorldToViewportPoint(minSection.transform.position).x;
            float maxX = Camera.main.WorldToViewportPoint(maxSection.transform.position).x;

            bool minVisible = minX >= -1 && minX <= 2 ? true : false;
            bool maxVisible = maxX >= -1 && maxX <= 2 ? true : false;

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
        public void CheckSectionsToCreate()
        {
            SectionCheck check = NeedNewSection();
            if (initialized && check.NeedNew)
            {
                int newSectionId = check.Section + check.Dir;
                int newSectionIdRight = check.SectionRight + check.DirRight;

                if (!instantiatedSections.Any(s => s.Id == newSectionId))
                {
                    CreateSection(newSectionId, check.Dir);
                }

                if (check.BothWays && !instantiatedSections.Any(s => s.Id == newSectionIdRight))
                {
                    CreateSection(newSectionIdRight, check.DirRight);
                }
            }
        }
        public void DestroyNotVisibleSections()
        {
            GameObject[] sections = GameObject.FindGameObjectsWithTag(Tags.Section);

            foreach (GameObject section in sections)
            {
                float sectionX = Camera.main.WorldToViewportPoint(section.transform.position).x;
                bool outsideView = sectionX < -1f || sectionX > 2f ? true : false;

                if (outsideView)
                {
                    int sectionId = GetSectionId(section);

                    instantiatedSections.Remove(instantiatedSections.Where(s => s.Id == sectionId).SingleOrDefault());
                    Destroy(section);
                }
            }
        }
        public void CheckWallsToRender(int sectionId)
        {
            Transform _wall = GameObject.Find("Section_" + sectionId).transform.Find(Tags.Wall);
            List<GameObject> walls = new List<GameObject>();

            foreach (Transform child in _wall)
            {
                if (child.tag == Tags.Wall && child.gameObject != null)
                {
                    walls.Add(child.gameObject);
                }
            }

            if (walls.Count > 0)
            {
                float maxWallY = Camera.main.WorldToViewportPoint(new Vector3(0, walls.Max(w => w.transform.position.y))).y;
                bool maxWallInView = maxWallY >= 0f && maxWallY <= 1f ? true : false;

                if (maxWallInView)
                {
                    GameObject wall = walls.Where(w => w.transform.position.y == walls.Max(ww => ww.transform.position.y)).SingleOrDefault();

                    string[] wallName = wall.name.Split('_');
                    int type = int.Parse(wallName[1]);
                    int amount = 1;
                    int offset = int.Parse(wallName[2]);

                    InstantiateWall(sectionId, 
                        wall.transform.position.x + .5f, sectionWidthTotal, wall.transform.position.z, 
                        type, amount, offset, true);
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
        public int GetSectionId(GameObject section)
        {
            return int.Parse(section.name.Split('_')[1]);
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
        private List<Section> instantiatedSections = new List<Section>();
        private GameObject _currentSection;
        private GameObject _currentBlocks;
        private System.Random rnd;

        private int sectionWidthTotal;
        private int sectionWidth;
        private float spacing;

        private bool initialized = false;

        private GameObject _factory;
        private BlockPool _pool;
        private Globals _globals;
        private World _world;
    }
}