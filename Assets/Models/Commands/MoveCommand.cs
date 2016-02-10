namespace Assets.Models.Commands
{
    public class MoveCommand : Command
    {
        public MoveCommand(string dir)
        {
            Direction = dir;
        }
        public override void Do(RobotArmController robotArm)
        {
            if (Direction == "right")
            {
                robotArm.MoveRight();
                world.MoveRight();
            }
            else if (Direction == "left")
            {
                robotArm.MoveLeft();
                world.MoveLeft();
            }

            // should set 'command.IsDone = true' after the robotArm transform/animation is finished 
            // and networkListener.ReturnMessage(s) should be after the 'command.IsDone'
            IsDone = true;
            networkListener.ReturnMessage("ok");
        }

        public string Direction { get; set; }
    }
}