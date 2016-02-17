using Assets.Scripts;
using UnityEngine;

namespace Assets.Models
{
    public class Command
    {
        public Command()
        {
            EventManager.AnimationIsDone += AnimationFinished;
            networkListener = GameObject.FindGameObjectWithTag("NetworkListener").GetComponent<NetworkListener>();
            world = GameObject.Find("Globals").GetComponent<Globals>().world;
            message = "ok";
            IsDone = false;
        }

        public virtual void Do(RobotArmController robotArm) { }

        public void AnimationFinished()
        {
            IsDone = true;
            networkListener.ReturnMessage(message);
            EventManager.AnimationIsDone -= AnimationFinished;
        }

        public void IsDoneReset()
        {
            IsDone = false;
        }

        public string message { get; set; }
        public bool IsDone { get; set; }

        protected World world;
        protected NetworkListener networkListener;
    }
}