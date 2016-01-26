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


        // for testing purposes
        else if (Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            commandRunner.Add(new MoveCommand("up"));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            commandRunner.Add(new SpeedCommand(10));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            commandRunner.Add(new SpeedCommand(20));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            commandRunner.Add(new SpeedCommand(30));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            commandRunner.Add(new SpeedCommand(40));
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
            commandRunner.Add(new SpeedCommand(90));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            commandRunner.Add(new SpeedCommand(100));
        }
    }
}