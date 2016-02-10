namespace Assets.Models.Commands
{
    public class DropCommand : Command
    {
        public override void Do(RobotArmController robotArm)
        {
            robotArm.Drop();
            world.Drop();

            // should set 'command.IsDone = true' after the robotArm transform/animation is finished 
            // and networkListener.ReturnMessage(s) should be after the 'command.IsDone'
            IsDone = true;
            networkListener.ReturnMessage("ok");
        }
    }
}