using Assets.Models.Commands;

namespace Assets.Models
{
    class CommandBuilder
    {
        public Command BuildCommand(string data)
        {
            string[] dataSplitted = new string[5];
            string instruction = "", parameter = "", user = "";

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
                if (parameter == "right" || parameter == "left")
                    return new MoveCommand(parameter);
                return new UnknownCommand(data);
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
                int speed;
                bool isInt = int.TryParse(parameter, out speed);

                if (isInt)
                    return new SpeedCommand(speed);
                return new UnknownCommand(data);
            }
            else if (instruction == "scan")
            {
                return new ScanCommand();
            }
            else if (instruction == "load")
            {
                user = dataSplitted.Length > 2 ? dataSplitted[2] : user;
                return new LoadLevelCommand(parameter, user);
            }
            else
            {
                return new UnknownCommand(data);
            }
        }
    }
}
