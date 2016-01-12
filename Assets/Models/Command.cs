using UnityEngine;

namespace Assets.Models
{
    public class Command
    {
        public Command()
        {
            networkListener = GameObject.FindGameObjectWithTag("NetworkListener").GetComponent<NetworkListener>();
        }

        protected NetworkListener networkListener; // should this be protected? or just public?
        public virtual bool Do(RobotArmController robotArm) { return false; }
    }
}