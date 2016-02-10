using Assets.Scripts;
using Assets.Scripts.WorldData;
using UnityEngine;

namespace Assets.Models
{
    public class Command
    {
        public Command()
        {
            networkListener = GameObject.FindGameObjectWithTag("NetworkListener").GetComponent<NetworkListener>();
            world = GameObject.Find("Globals").GetComponent<Globals>().world;
            IsDone = false;
        }

        public virtual void Do(RobotArmController robotArm) { }
        public void Stop()
        {
            IsDone = true;
        }
        public bool IsDone { get; set; }

        protected World world;
        protected NetworkListener networkListener;
    }
}