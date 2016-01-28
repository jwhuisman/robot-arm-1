using UnityEngine;

namespace Assets.Scripts
{
    public class View : MonoBehaviour
    {
        public GameObject floorModel;
        public GameObject wallModel;

        public void GenerateFactorySection(float x)
        {
            GameObject floor = Instantiate(floorModel);
            GameObject wall = Instantiate(wallModel);

            floor.transform.parent = GameObject.Find("Factory").transform;
            wall.transform.parent = GameObject.Find("Factory").transform;

            floor.name = "floor-" + x;
            wall.name = "wall-" + x;

            float width = floor.GetComponentInChildren<Renderer>().bounds.size.x;
            float posX = x * width;
            float posZ = width / 2;

            floor.transform.position = new Vector3(posX, 0);
            wall.transform.position = new Vector3(posX, 0, posZ);
        }
    }
}
