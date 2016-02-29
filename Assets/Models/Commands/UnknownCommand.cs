﻿using Assets.Scripts.View;

namespace Assets.Models.Commands
{
    public class UnknownCommand : Command
    {
        public UnknownCommand(string instruction)
        {
            Instruction = instruction;
        }

        public override void Do(RobotArm robotArm)
        {
            message = "I dont know what \"" + Instruction + "\" means";
            IsDone = true;

            networkListener.ReturnMessage(message);
        }

        public string Instruction { get; set; }   
    }
}