using Assets.Scripts;
using Assets.Scripts.View;
using Assets.Scripts.WorldData;
using System;
using UnityEngine;

namespace Assets.Models
{
    public class Command
    {
        public Command()
        {
            networkListener = GameObject.Find(Tags.Scripts).GetComponent<NetworkListener>();
            world = GameObject.Find(Tags.Globals).GetComponent<Globals>().world;
            //EventManager.AnimationIsDone += AnimationFinished;
            message = "ok";
            IsDone = false;
        }

        public virtual void Do(RobotArm robotArm) { }

        public void AnimationFinished(object sender, EventArgs e)
        {
            IsDone = true;
            networkListener.ReturnMessage(message);
        }
        public string message { get; set; }
        public bool IsDone { get; set; }

        protected World world;
        protected NetworkListener networkListener;
    }
}