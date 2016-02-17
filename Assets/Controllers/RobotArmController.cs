using Assets.Scripts;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RobotArmController : MonoBehaviour
{

    //---------------------------------------------------------//
    //   this Controller should probably be taken apart.       //
    //   the needle / pointer stuff should be in the view      //
    //   the moveLeft, grab etc. should be in RobotArm (class) //
    //   for that we also need to change the commands          //
    //---------------------------------------------------------//

    public GameObject robotArm;

    // test text
    public Text speedText;
    
    private float mapBoundaryTop;

    // speed meter stuff
    public GameObject needle;
    public float angleY;
    public float angleZ;
    public float startAngle;
    public float currentAngle;
    public float targetAngle;
    public float minAngle = 0;
    public float maxAngle = 240;
    public float rotSpeed = 3f;
    public bool rotateNeedle = false;
    public Vector3 targetPosition;


    public void Start()
    {
        _globals = GameObject.Find("Globals").GetComponent<Globals>();
        _view = GameObject.Find("View").GetComponent<View>();
        _animator = GetComponent<Animator>();

        _world = _globals.world;

        UpdateArmHeight();

        InitNeedle();
    }

    public void Update()
    {
        if (rotateNeedle)
        {
            RotateNeedleTowards();
        }
    }

    // set the height till where the arm should go up
    public void UpdateArmHeight()
    {
        mapBoundaryTop = GetHighestCubeY() + .6f;

        _world.RobotArm.Y = GetHighestCubeY();
    }
    public int GetHighestCubeY()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        int offsetY = 2;
        int y = 0;

        if (blocks.Length > 0)
        {
            y = (int)blocks.Max(c => c.transform.position.y);
        }

        return y + offsetY;
    }

    // speed meter stuff
    public void UpdateSpeed(float speed)
    {
        //float time = timeTakenDuringLerp;

        //if (speed <= 100 && speed >= 0)
        //{
        //    time = (100f - speed) / 100f;
        //    speedText.text = "Speed: " + time + " seconds";

        //    SetSpeedMeter(time);

        //    timeTakenDuringLerp = time;
        //}
    }
    public void InitNeedle()
    {
        needle = GameObject.Find("Needle");
        angleY = needle.transform.rotation.eulerAngles.y;
        angleZ = needle.transform.rotation.eulerAngles.z;

        //SetNeedle(timeTakenDuringLerp);
    }
    public void SetSpeedMeter(float time)
    {
        startAngle = (!float.IsNaN(currentAngle)) ? currentAngle : needle.transform.eulerAngles.x;
        currentAngle = startAngle;
        targetAngle = GetAngle(time);

        if (targetAngle != startAngle)
        {
            rotateNeedle = true;
        }
    }
    public void RotateNeedleTowards()
    {
        float speed = rotSpeed * (Math.Abs(targetAngle - startAngle) / 40);
        bool left = (startAngle < targetAngle) ? true : false;

        currentAngle = (left) ? currentAngle + speed : currentAngle - speed;

        needle.transform.rotation = Quaternion.Euler(0, 0, currentAngle);

        if ((left && currentAngle >= targetAngle) || (!left && currentAngle <= targetAngle))
        {
            needle.transform.rotation = Quaternion.Euler(0, 0, targetAngle);
            rotateNeedle = false;
        }
    }
    public void SetNeedle(float time)
    {
        float a = GetAngle(time);
        currentAngle = a;
        needle.transform.rotation = Quaternion.Euler(0, 0, a);
    }
    public float GetAngle(float time)
    {
        return -(maxAngle - (maxAngle * time));
    }

    // animator stuff
    public void GrabStateScript()
    {
        if (_world.RobotArm.X != robotArm.transform.position.x)
        {
            Debug.Log("ERROR: RobotArm View does not have the same position as the RobotArm World.");
        }

        GameObject blockBeneath = _view.FindBlockAtX(_world.RobotArm.X);
        blockBeneath.transform.parent = robotArm.transform;
    }
    public void PositionCalculateStateScript()
    {
        mapBoundaryTop = GetHighestCubeY();
        targetPosition = new Vector3(transform.position.x, mapBoundaryTop, transform.position.z);

        _animator.SetTrigger("Next");
    }

    
    // these should trigger the animations
    public void MoveLeft()
    {

        //_world.MoveLeft();
    }
    public void MoveRight()
    {
        //_world.MoveRight();
    }
    public void MoveUp()
    {
        //_world.MoveUp();
    }
    public void Grab()
    {
        bool blockDetected = false;
        float positionY = GetHighestCubeY();

        foreach (Transform child in transform)
        {
            if (child.tag == "Block")
            {
                blockDetected = true;
            }
        }

        // if world.RobotArm holds a block, then View.RobotArm also needs to pick up a block.
        if (_view.FindBlockAtX(_world.RobotArm.X) == null)
        {
            // positionY should be changed to the position of the assembly line (findWithTag)
            positionY = 1.5f;
            _animator.SetBool("GoPickUp", false);
        }
        else if (_world.RobotArm.Holding && blockDetected)
        {
            positionY = _view.FindBlockAtX(_world.RobotArm.X).transform.position.y + 2.5f;
            _animator.SetBool("GoPickUp", false);
        }
        else if (_world.RobotArm.Holding)
        {
            positionY = _view.FindBlockAtX(_world.RobotArm.X).transform.position.y + 1.5f;
            _animator.SetBool("GoPickUp", true);
        }

        targetPosition = new Vector3(transform.position.x, positionY, transform.position.z);
        this._animator.SetTrigger("PickUp animation");
    }
    public void Drop()
    {
        //_world.Drop();
    }
    public void Scan()
    {
        this._animator.SetTrigger("Scan animation");
    }

    private Globals  _globals;
    private World    _world;
    private Animator _animator;
    private View     _view;
}