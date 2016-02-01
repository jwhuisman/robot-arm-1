using UnityEngine;

namespace Assets.Scripts
{
    public class View : MonoBehaviour
    {
        public GameObject floorModel;
        public GameObject wallModel;

        public void Start()
        {
            for (int i = -2; i <= 2; i++)
            {
                GenerateFactorySection(i);
            }
        }


        public void GenerateFactorySection(float sectionId)
        {
            float width = 15f;
            float posX = (width / 2) + (sectionId * width) - .5f;
            float posZ = (width / 2) - .5f;

            GenerateFloor(sectionId, posX);
            GenerateWall(sectionId, posX, posZ);
        }

        public void GenerateFloor(float id, float x)
        {
            GameObject floor = Instantiate(floorModel);
            floor.transform.parent = GameObject.Find("Factory").transform;
            floor.name = "floor-(" + id + ")";
            floor.transform.position = new Vector3(x, -.5f);
        }
        public void GenerateWall(float id, float x, float z)
        {
            GameObject wall = Instantiate(wallModel);
            wall.transform.parent = GameObject.Find("Factory").transform;
            wall.name = "wall-(" + id + ")";
            wall.transform.position = new Vector3(x, -.5f, z);
        }
    }
}
