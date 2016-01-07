using UnityEngine;

public class InputListener : MonoBehaviour
{
    public RobotArmController _RobotArmController;

    void Start()
    {
        Debug.Log("InputListener.cs is used!");
    }

    void Update()
    {
        string msg = "";
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            msg = "put down";
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            msg = "pick up";
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            msg = "move left";
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            msg = "move right";
        }

        if (msg != "")
        {
            _RobotArmController.Actions(msg);
            Debug.Log(string.Format("Message received: {0}", msg));
        }
    }
}