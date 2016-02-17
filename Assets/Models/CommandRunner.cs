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
            if (currentCmd != null && currentCmd.IsDone)
            {
                if (Queue.Count != 0)
                {
                    Debug.Log("Next one!");
                    //Next();
                }
            }

            if (firstCommand && Queue.Count != 0)
            {
                firstCommand = false;
                //Next();
            }
        }

        public void Add(Command cmd)
        {
            Queue.Enqueue(cmd);
        }

        public void Next()
        {
            Debug.Log("Next command!");
            currentCmd = Queue.Dequeue();
            currentCmd.Do(robotArmController);
            
        }

        private bool firstCommand = true;
        private Command currentCmd;
        private Queue<Command> Queue;
    }
}
