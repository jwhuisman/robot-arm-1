﻿using UnityEngine;

namespace Assets.Models
{
    public class Command
    {
        public Command()
        {
            networkListener = GameObject.FindGameObjectWithTag("NetworkListener").GetComponent<NetworkListener>();
            IsDone = false;
        }

        public virtual void Do(RobotArmController robotArm) { }
        public void Stop()
        {
            IsDone = true;
        }
        public bool IsDone { get; set; }

        protected NetworkListener networkListener;
    }
}