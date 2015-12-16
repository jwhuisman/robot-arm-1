using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class CommandRunner : MonoBehaviour
    {
        public void QueueCommand(Command command)
        {
            
        }

        private Queue<Command> _commands;
    }
}