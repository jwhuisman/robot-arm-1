using Assets.Models;
using Assets.Models.Commands;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    void Start()
    {
        Debug.Log("InputListener.cs is used!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (holding)
            {
                holding = false;
                commandRunner.Add(new GrabCommand("put"));
            }
            else
            {
                holding = true;
                commandRunner.Add(new GrabCommand("grab"));
            }
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

    private bool holding = false;
    public CommandRunner commandRunner;
}