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
            commandRunner.Add(new SpeedCommand(0));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            commandRunner.Add(new SpeedCommand(50));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            commandRunner.Add(new SpeedCommand(100));
        }
    }
}