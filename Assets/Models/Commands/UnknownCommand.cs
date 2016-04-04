using Assets.Scripts.View;

namespace Assets.Models.Commands
{
    public class UnknownCommand : Command
    {
        public UnknownCommand(string instruction)
        {
            Instruction = instruction;
        }

        public override void Do(RobotArm robotArm, SpeedMeter speedMeter, StatsCounter statsCounter)
        {
            message = "nope";
            IsDone = true;

            networkListener.ReturnMessage(message);
        }

        public string Instruction { get; set; }   
    }
}