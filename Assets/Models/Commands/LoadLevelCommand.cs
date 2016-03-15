using Assets.Scripts.View;
using UnityEngine;

namespace Assets.Models.Commands
{
    public class LoadLevelCommand : Command
    {
        public LoadLevelCommand(int level = 0)
        {
            Level = level;
        }

        public override void Do(RobotArm robotArm)
        {
            world.LoadLevel(Level);

            GameObject.Find(Tags.View).GetComponent<SectionBuilder>().Reload();

            IsDone = true;

            networkListener.ReturnMessage(message);
        }

        public int Level { get; set; }
    }
}