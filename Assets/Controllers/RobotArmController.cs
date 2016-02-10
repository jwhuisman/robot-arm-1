using Assets.Scripts;
using Assets.Scripts.WorldData;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RobotArmController : MonoBehaviour
{
    public void Start()
    {
        _globals = GameObject.Find("Globals").GetComponent<Globals>();
        _view = GameObject.Find("View").GetComponent<View>();
        _world = _globals.world;
    }

    public void UpdateArmHeight()
    {
        _world.RobotArm.Y = GetHighestCubeY();
    }
    public int GetHighestCubeY()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block").Where(b => b.name != "Block-"+_world.RobotArm.HoldingBlock.Id).ToArray();
        int offsetY = 3;
        int y = 0;

        if (blocks.Length > 0)
        {
            y = (int)blocks.Max(c => c.transform.position.y);
        }

        return y + offsetY;
    }


    // these should trigger the animations
    public void MoveLeft()
    {
        
    }
    public void MoveRight()
    {
        
    }
    public void Grab()
    {

        UpdateArmHeight();
    }
    public void Drop()
    {

        UpdateArmHeight();
    }
    public void Scan()
    {
        
    }
    public void SetSpeed(int speed)
    {
        _view.SetSpeed(speed);
    }


    private Globals _globals;
    private World _world;
    private View _view;
}