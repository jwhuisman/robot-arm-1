using Assets.Models.Commands;
using Assets.Scripts;
using Assets.Scripts.View;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Models
{
    public class CommandRunner : MonoBehaviour
    {
        public RobotArm robotArm;
        public SpeedMeter speedMeter;
        public StatsCounter statsCounter;

        public CommandRunner()
        {
            Queue = new Queue<Command>();
        }

        public void Update()
        {
            if (statsCounter.Queued != Queue.Count)
            {
                statsCounter.Queued = Queue.Count;
            }

            if (currentCmd != null && currentCmd.IsDone)
            {
                if (Queue.Count != 0)
                {
                    Next();
                }
            }

            if (firstCommand && Queue.Count != 0)
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
            if (currentCmd != null)
            {
                robotArm.AnimationIsDone -= currentCmd.AnimationFinished;
            }

            currentCmd = Queue.Dequeue();
            robotArm.AnimationIsDone += currentCmd.AnimationFinished;
            
            currentCmd.Do(robotArm, speedMeter, statsCounter);

            if (robotArm._originalSpeed == 100)
            {
                // When the world finishes the last command he needs to update
                // the current position it needs to be at.
                robotArm.SetTargetPosition();
            }

            if (currentCmd is LoadLevelCommand)
            {
                Levels levels = GameObject.Find(Tags.Globals).GetComponent<Globals>().world.levels;

                levels.DownloadAgain();
            }
        }

        private bool firstCommand = true;
        private Command currentCmd;
        private Queue<Command> Queue;
    }
}
