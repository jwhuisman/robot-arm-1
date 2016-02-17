namespace Assets.Models.Commands
{
    public class GrabCommand : Command
    {
        public override void Do(RobotArmController robotArm)
        {
            world.Grab();
            robotArm.Grab();
        }
    }
}