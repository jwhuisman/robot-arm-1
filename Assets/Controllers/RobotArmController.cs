using Assets.Scripts;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RobotArmController : MonoBehaviour
{
    public GameObject robotArm;

    //---------------------------------------------------------//
    //   this Controller should probably be taken apart.       //
    //   the needle / pointer stuff should be in the view      //
    //   the moveLeft, grab etc. should be in RobotArm (class) //
    //   for that we also need to change the commands          //
    //---------------------------------------------------------//


    // test text
    public Text speedText;

    // lerping related
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private bool _isLerping;
    private float percentageComplete;
    private float _timeStartedLerping;
    public  float timeTakenDuringLerp;

    // pick up, put down a block related.
    public  bool currentlyHolding = false;
    private bool goPickUpBlock = false;
    private bool goPutDownBlock = false;
    private bool goUpFromPlane = false;
    
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


    public void Start()
    {
        _globals = GameObject.Find("Globals").GetComponent<Globals>();
        _world = _globals.world;

        UpdateArmHeight();

        timeTakenDuringLerp = 0.5f;
        speedText.text = "Speed: " + timeTakenDuringLerp + " seconds";
       
        InitNeedle();
    }


    public void Update()
    {   
        // Actions after the animation "going down" is completed.
        if (percentageComplete == 1f && goPickUpBlock)
        {
            goPickUpBlock = false;
            GrabRelease(true);
        }
        if (percentageComplete == 1f && goPutDownBlock)
        {
            goPutDownBlock = false;
            GrabRelease(false);
        }
        if (percentageComplete == 1f && goUpFromPlane)
        {
            goUpFromPlane = false;
            StartLerping(Vector3.up, mapBoundaryTop);
        }

        if (rotateNeedle)
        {
            RotateNeedleTowards();
        }
    }
    public void FixedUpdate()
    {
        if (_isLerping)
        {
            float timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / timeTakenDuringLerp;
            
            robotArm.transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);

            if (percentageComplete >= 1.0f)
            {
                UpdateArmHeight();

                percentageComplete = 1.0f;
                _isLerping = false;
            }
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
        float time = timeTakenDuringLerp;

        if (speed <= 100 && speed >= 0)
        {
            time = (100f - speed) / 100f;
            speedText.text = "Speed: " + time + " seconds";

            SetSpeedMeter(time);

            timeTakenDuringLerp = time;
        }
    }
    public void InitNeedle()
    {
        needle = GameObject.Find("Needle");
        angleY = needle.transform.rotation.eulerAngles.y;
        angleZ = needle.transform.rotation.eulerAngles.z;

        SetNeedle(timeTakenDuringLerp);
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
    }
    public void Drop()
    {
        _world.Drop();
    }


    // old unused stuff
    public void StartLerping(Vector3 direction, float spaces)
    {
        if (!_isLerping || percentageComplete >= 1.0f)
        {
            _isLerping = true;
            _timeStartedLerping = Time.time;
            _startPosition = robotArm.transform.position;
            _endPosition = robotArm.transform.position + direction * spaces;
        }
    }
    public void StartPickUpPutDown(bool instruction)
    {
        RaycastHit raycastNearestDownwardObject;
        if (Physics.Raycast(robotArm.transform.position, Vector3.down, out raycastNearestDownwardObject))
        {
            // if : the plane is raycasted the robotArm will go down, touch it, and go back up.
            // else : instruction = true : pick up a block. false : put down a block.

            // Note: By resetting the precentage we ensure everything goes consecutively and the robotArm will go up after completing the "going down" animation.
            //if (raycastNearestDownwardObject.transform.name == plane.name)
            //{
            //    // Note: there is nothing to pick up because the robotArm raycasted the plane and the robotArm doesn't hold a block.
            //    float distanceTillHitPlane = raycastNearestDownwardObject.distance - (robotArm.transform.localScale.y / 2 ) ;
            //    StartLerping(Vector3.down, distanceTillHitPlane);

            //    percentageComplete = 0f;
            //    goUpFromPlane = true;
            //}
            if (instruction && !currentlyHolding)
            {
                float distanceTillHitBlock = raycastNearestDownwardObject.distance - (robotArm.transform.localScale.y / 2);
                StartLerping(Vector3.down, distanceTillHitBlock);
                
                percentageComplete = 0f;
                goPickUpBlock = true;
            }
            else if (!instruction && currentlyHolding)
            {
                // casts a raycast from the point of the block, downwards.
                RaycastHit underneathBlock;
                if (Physics.Raycast(raycastNearestDownwardObject.transform.position, Vector3.down, out underneathBlock))
                {
                    float distanceUnderneathBlock = underneathBlock.distance - (raycastNearestDownwardObject.transform.lossyScale.y / 2 );
                    StartLerping(Vector3.down, distanceUnderneathBlock); 

                    percentageComplete = 0f;
                    goPutDownBlock = true;
                }
            }
        }
    }
    public void GrabRelease(bool instruction)
    {
        RaycastHit blockDirectlyUnderneath;
        if (Physics.Raycast(robotArm.transform.position, Vector3.down, out blockDirectlyUnderneath))
        {
            var block = GameObject.Find(blockDirectlyUnderneath.transform.name);

            // instruction =  If true : grab a block. If false : release a block.  
            if (instruction && !currentlyHolding)
            {
                if (block.transform.parent = robotArm.transform)
                {
                    currentlyHolding = true;
                }
            } else if (!instruction && currentlyHolding) {
                if (block.transform.parent = GameObject.Find("Cubes").transform)
                {
                    currentlyHolding = false;
                }
            }

            // after grabbing or releasing a block, go up to the mapBoundaryTop.
            float distanceMapBoundary = mapBoundaryTop - robotArm.transform.position.y;
            StartLerping(Vector3.up, distanceMapBoundary);
        }
    }


    private Globals _globals;
    private World _world;
}