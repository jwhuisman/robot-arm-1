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
                world.MoveRight();
                robotArm.MoveRight();
            }
            else if (Direction == "left")
            {
                world.MoveLeft();
                robotArm.MoveLeft();
            }

            // should set 'command.IsDone = true' after the robotArm transform/animation is finished 
            // and networkListener.ReturnMessage(s) should be after the 'command.IsDone'
            IsDone = true;
            networkListener.ReturnMessage("ok");
        }

        public string Direction { get; set; }
    }
}