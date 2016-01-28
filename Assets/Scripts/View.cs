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


        public void GenerateFactorySection(float x)
        {
            GameObject floor = Instantiate(floorModel);
            GameObject wall = Instantiate(wallModel);

            floor.transform.parent = GameObject.Find("Factory").transform;
            wall.transform.parent = GameObject.Find("Factory").transform;

            floor.name = "floor-(" + x + ")";
            wall.name = "wall-(" + x + ")";

            float width = floor.GetComponentInChildren<Renderer>().bounds.size.x;
            float posX = (width / 2) + (x * width) - .5f;
            float posZ = (width / 2) - .5f;

            floor.transform.position = new Vector3(posX, -.5f);
            wall.transform.position = new Vector3(posX, -.5f, posZ);
        }
    }
}
