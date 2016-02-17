namespace Assets.Models.Commands
{
    public class ScanCommand : Command
    {
        public override void Do(RobotArmController robotArm)
        {
            message = world.Scan();
            robotArm.Scan();
        }
    }
}