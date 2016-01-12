using UnityEngine;

namespace Assets.Models
{
    public class Command
    {
        public NetworkListener nListener;

        public Command()
        {
            nListener = GameObject.FindGameObjectWithTag("NetworkListener").GetComponent<NetworkListener>();
        }

        public virtual bool Do(RobotArmController robotArm) { return false; }
    }
}