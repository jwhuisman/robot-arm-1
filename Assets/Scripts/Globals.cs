using Assets.Scripts.WorldData;
using UnityEngine;

namespace Assets.Scripts
{
    class Globals : MonoBehaviour
    {
        public World world;

        public void Start()
        {
            world = new World();
        }
    }
}