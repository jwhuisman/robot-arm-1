using Assets.Scripts;
using Assets.Scripts.View;
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
            message = "ok";
        }

        public virtual void Do(RobotArm robotArm) { }
        public void Stop()
        {
            networkListener.ReturnMessage(message);
            IsDone = true;
        }
        public bool IsDone { get; set; }
        protected string message { get; set; }

        protected World world;
        protected NetworkListener networkListener;
    }
}