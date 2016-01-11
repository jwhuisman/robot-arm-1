using Assets.Models.Commands;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Models
{
    public class CommandRunner : MonoBehaviour
    {
        public RobotArmController robotArmController;

        public CommandRunner()
        {
            Queue = new Queue<Command>();
        }

        public void Update()
        {
            if (Queue.Count != 0 && commandFinished)
            {
                commandFinished = false;

                NextCommand();
            }

            if (Queue.Count != 0 && firstCommand && !commandFinished)
            {
                firstCommand = false;

                NextCommand();
            }
        }

        public void Add(Command cmd)
        {
            Queue.Enqueue(cmd);
        }
        public void NextCommand()
        {
            Command cmd = Queue.Dequeue();

            cmd.Do(robotArmController);

            commandFinished = true;
        }

        private bool firstCommand = true;
        private bool commandFinished = false;
        private Queue<Command> Queue;
    }
}
