using Assets.Scripts.WorldData;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.View
{
    public class RobotArm : MonoBehaviour
    {
        public void Start()
        {
            _globals = GameObject.Find("Globals").GetComponent<Globals>();
            _view = GameObject.Find("View").GetComponent<View>();
            _world = _globals.world;
        }

        public void UpdateArmHeight()
        {
            _world.RobotArm.Y = GetHighestCubeY();
        }
        public int GetHighestCubeY()
        {
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block").Where(b => b.name != "Block-"+_world.RobotArm.HoldingBlock.Id).ToArray();
            int offsetY = 3;
            int y = 0;

            if (blocks.Length > 0)
            {
                y = (int)blocks.Max(c => c.transform.position.y);
            }

            return y + offsetY;
        }

        public void MoveLeft()
        {
            _view.UpdateView();
        }
        public void MoveRight()
        {
            _view.UpdateView();
        }
        public void Grab()
        {
            UpdateArmHeight();

            _view.UpdateView();
        }
        public void Drop()
        {
            UpdateArmHeight();

            _view.UpdateView();
        }
        public void Scan()
        {

        }
        public void SetSpeed(int speed)
        {
            float time = 0;

            if (speed <= 100 && speed >= 0)
            {
                time = (100f - speed) / 100f;

                armSpeed = time;
            }

            _view.speedMeter.SetSpeed(time);
        }

        private float armSpeed;

        private Globals _globals;
        private World _world;
        private View _view;
    }
}