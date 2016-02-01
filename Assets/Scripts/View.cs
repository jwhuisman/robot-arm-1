using UnityEngine;

namespace Assets.Scripts
{
    public class View : MonoBehaviour
    {
        public GameObject assemblyLineModel;
        public GameObject floorModel;
        public GameObject wallModel;

        public void Start()
        {
            _factory = GameObject.Find("Factory");

            for (int i = -3; i <= 3; i++)
            {
                GenerateFactorySection(i);
            }
        }


        public void GenerateFactorySection(float sectionId)
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

        public void GenerateAssemblyLine(float id, float x)
        {
            GameObject assembly = Instantiate(assemblyLineModel);
            assembly.transform.parent = _factory.transform;
            assembly.name = "assembly-(" + id + ")";
            assembly.transform.position = new Vector3(x - .5f, -.5f);
        }
        public void GenerateFloor(float id, int amount, float x, float z)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject floor = Instantiate(floorModel);
                floor.transform.parent = _factory.transform;
                floor.name = "floor-(" + id + "-" + i + ")";
                floor.transform.position = new Vector3(x - .5f, -.5f, z * (i + 1));
            }
        }
        public void GenerateWall(float id, int amount, float x, float y, float z)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject wall = Instantiate(wallModel);
                wall.transform.parent = _factory.transform;
                wall.name = "wall-(" + id + "-" + i + ")";
                wall.transform.position = new Vector3(x - .5f, (y * i) - .5f, z);
            }
        }

        private GameObject _factory;
    }
}
