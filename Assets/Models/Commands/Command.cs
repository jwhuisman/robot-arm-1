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
            networkListener = GameObject.FindWithTag("Scripts").GetComponent<NetworkListener>();
            world = GameObject.FindWithTag("Scripts").GetComponent<Globals>().world;

            message = "ok";
            IsDone = false;
        }

        public virtual void Do(RobotArm robotArm, SpeedMeter speedMeter, StatsCounter statsCounter) { }

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