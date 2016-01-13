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

                Next();
            }

            if (firstCommand && Queue.Count != 0 && !commandFinished)
            {
                firstCommand = false;

                Next();
            }
        }

        public void Add(Command cmd)
        {
            Queue.Enqueue(cmd);
        }
        public void Next()
        {
            Command cmd = Queue.Dequeue();

            if (cmd.Do(robotArmController))
            {
                // command/animation finished -> ready for next command
                commandFinished = true;
            }
        }

        private bool firstCommand = true;
        private bool commandFinished = false;
        private Queue<Command> Queue;
    }
}
