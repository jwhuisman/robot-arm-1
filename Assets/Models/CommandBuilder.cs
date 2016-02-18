using Assets.Models.Commands;

namespace Assets.Models
{
    class CommandBuilder
    {
        public Command BuildCommand(string data)
        {
            string[] dataSplitted = new string[2];
            string instruction = "", parameter = "";

            if (data.IndexOf(' ') >= 0)
            {
                dataSplitted = data.Split(' ');
                instruction = dataSplitted[0];
                parameter = dataSplitted[1];
            } else
            {
                instruction = data;
            }

            if (instruction == "move")
            {
                return new MoveCommand(parameter);
            }
            else if (instruction == "grab")
            {
                return new GrabCommand();
            }
            else if (instruction == "drop")
            {
                return new DropCommand();
            }
            else if (instruction == "speed")
            {
                return new SpeedCommand(int.Parse(parameter));
            }
            else if (instruction == "scan")
            {
                return new ScanCommand();
            }
            else
            {
                return new UnknownCommand(data);
            }
        }
    }
}
