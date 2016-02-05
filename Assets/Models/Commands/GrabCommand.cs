namespace Assets.Models.Commands
{
    public class GrabCommand : Command
    {
        public override void Do(RobotArmController robotArm)
        {
            robotArm.Grab();

            // should set 'command.IsDone = true' after the robotArm transform/animation is finished 
            // and networkListener.ReturnMessage(s) should be after the 'command.IsDone'
            IsDone = true;
            networkListener.ReturnMessage("ok");
        }
    }
}