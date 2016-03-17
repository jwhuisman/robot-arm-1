using Assets.Scripts.View;
using UnityEngine;

namespace Assets.Models.Commands
{
    public class LoadLevelCommand : Command
    {
        public LoadLevelCommand(string name)
        {
            LevelName = name;
        }

        public override void Do(RobotArm robotArm)
        {
            bool levelExists = world.LoadLevel(LevelName);
            GameObject.Find(Tags.View).GetComponent<SectionBuilder>().Reload();

            IsDone = true;

            if (!levelExists)
            {
                message = "wrong";
            }

            networkListener.ReturnMessage(message);
        }

        public string LevelName { get; set; }
    }
}