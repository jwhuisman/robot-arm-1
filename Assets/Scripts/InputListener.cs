using Assets.Models;
using Assets.Models.Commands;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    public CommandRunner commandRunner;

    void Start()
    {
        Debug.Log("InputListener.cs is used!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

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


        // for testing purposes
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            commandRunner.Add(new SpeedCommand(1));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            commandRunner.Add(new SpeedCommand(2));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            commandRunner.Add(new SpeedCommand(3));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            commandRunner.Add(new SpeedCommand(4));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            commandRunner.Add(new SpeedCommand(5));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            commandRunner.Add(new SpeedCommand(6));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            commandRunner.Add(new SpeedCommand(7));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            commandRunner.Add(new SpeedCommand(8));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            commandRunner.Add(new SpeedCommand(9));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            commandRunner.Add(new SpeedCommand(10));
        }
    }
}