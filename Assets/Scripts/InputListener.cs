using Assets.Models;
using Assets.Models.Commands;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    public CommandRunner commandRunner;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            commandRunner.Add(new DropCommand());
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            commandRunner.Add(new GrabCommand());
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            commandRunner.Add(new MoveCommand("left"));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            commandRunner.Add(new MoveCommand("right"));
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            commandRunner.Add(new ScanCommand());
        }

        else if (Input.GetKeyDown(KeyCode.F1))
        {
            commandRunner.Add(new LoadLevelCommand("oefening 1"));
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            commandRunner.Add(new LoadLevelCommand("oefening 2"));
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            commandRunner.Add(new LoadLevelCommand("oefening 3"));
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            commandRunner.Add(new LoadLevelCommand("oefening 4"));
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            commandRunner.Add(new LoadLevelCommand("oefening 5"));
        }
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            commandRunner.Add(new LoadLevelCommand("oefening 6"));
        }
        else if (Input.GetKeyDown(KeyCode.F7))
        {
            commandRunner.Add(new LoadLevelCommand("oefening 7"));
        }


        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            commandRunner.Add(new SpeedCommand(0));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            commandRunner.Add(new SpeedCommand(1));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            commandRunner.Add(new SpeedCommand(10));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            commandRunner.Add(new SpeedCommand(20));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            commandRunner.Add(new SpeedCommand(50));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            commandRunner.Add(new SpeedCommand(60));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            commandRunner.Add(new SpeedCommand(70));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            commandRunner.Add(new SpeedCommand(80));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            commandRunner.Add(new SpeedCommand(99));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            commandRunner.Add(new SpeedCommand(100));
        }
    }
}