namespace Assets.Models.Commands
{
    public class GrabCommand : Command
    {
        public override void Do(RobotArmController robotArm)
        {
            string s = "Pick up a block";
            robotArm.StartPickUpPutDown(true);

            // should set 'command.IsDone = true' after the robotArm transform/animation is finished 
            // and networkListener.ReturnMessage(s) should be after the 'command.IsDone'
            IsDone = true;
            robotArm.text.text = s;
            networkListener.ReturnMessage(s);
        }
    }
}