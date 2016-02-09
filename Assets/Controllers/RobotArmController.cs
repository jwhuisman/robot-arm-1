using Assets.Scripts;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RobotArmController : MonoBehaviour
{
    public GameObject robotArm;
    public Text speedText;
    public  float armSpeed;

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


    public void Start()
    {
        _globals = GameObject.Find("Globals").GetComponent<Globals>();
        _world = _globals.world;

        armSpeed = 0.5f;
        speedText.text = "Speed: " + armSpeed + " seconds";
       
        InitNeedle();
    }

    public void Update()
    {   
        if (rotateNeedle)
        {
            RotateNeedleTowards();
        }
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

    // speed meter stuff
    public void UpdateSpeed(float speed)
    {
        float time = armSpeed;

        if (speed <= 100 && speed >= 0)
        {
            time = (100f - speed) / 100f;
            speedText.text = "Speed: " + time + " seconds";

            SetSpeedMeter(time);

            armSpeed = time;
        }
    }
    public void InitNeedle()
    {
        needle = GameObject.Find("Needle");
        angleY = needle.transform.rotation.eulerAngles.y;
        angleZ = needle.transform.rotation.eulerAngles.z;

        SetNeedle(armSpeed);
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


    // these should trigger the animations
    public void MoveLeft()
    {
        _world.MoveLeft();
    }
    public void MoveRight()
    {
        _world.MoveRight();
    }
    public void MoveUp()
    {
        _world.MoveUp();
    }
    public void Grab()
    {
        _world.Grab();
        UpdateArmHeight();
    }
    public void Drop()
    {
        _world.Drop();
        UpdateArmHeight();
    }


    private Globals _globals;
    private World _world;
}