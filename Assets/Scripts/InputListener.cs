using Assets.Models;
using Assets.Models.Commands;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    public RobotArmController robotArm;
    public CommandRunner commandRunner;


    void Start()
    {
        Debug.Log("InputListener.cs is used!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (robotArm.currentlyHolding)
            {
                commandRunner.Add(new GrabCommand("put"));
            }
            else
            {
                commandRunner.Add(new GrabCommand("grab"));
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) // up = for testing purposes
        { 
            commandRunner.Add(new MoveCommand("up"));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            commandRunner.Add(new MoveCommand("left"));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            commandRunner.Add(new MoveCommand("right"));
        }
    }
}