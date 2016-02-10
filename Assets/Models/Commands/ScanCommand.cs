namespace Assets.Models.Commands
{
    public class ScanCommand : Command
    {
        public override void Do(RobotArmController robotArm)
        {
            string color = world.Scan();
            robotArm.Scan();

            // should set 'command.IsDone = true' after the robotArm animation is finished 
            // and networkListener.ReturnMessage(s) should be after the 'command.IsDone'
            IsDone = true;
            networkListener.ReturnMessage("block color: " + color);
        }
    }
}