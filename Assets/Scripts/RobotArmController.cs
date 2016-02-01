using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RobotArmController : MonoBehaviour
{
    public GameObject robotArm;
    public GameObject plane;
    public GameObject cubes;

    // world stuff
    public Text text;
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
    public GameObject meterPointer;
    public float angleY;
    public float angleZ;
    public float startAngle;
    public float currentAngle;
    public float targetAngle;
    public float minAngle = 0;
    public float maxAngle = 240;
    public float rotSpeed = 3f;
    public bool rotateNeedle = false;


    void Start()
    {
        UpdateArmHeight();

        timeTakenDuringLerp = 0.5f;
        speedText.text = "Speed: " + timeTakenDuringLerp + " seconds";

        meterPointer = GameObject.FindGameObjectWithTag("SpeedMeterPointer");
        angleY = meterPointer.transform.rotation.eulerAngles.y;
        angleZ = meterPointer.transform.rotation.eulerAngles.z;

        cubes = GameObject.Find("Cubes");

        SetNeedle(timeTakenDuringLerp);
    }
	
	void Update()
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
    void FixedUpdate()
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

    public void UpdateArmHeight()
    {
        mapBoundaryTop = GetHighestCubeY();
    }
    public float GetHighestCubeY()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
        float offsetY = 2f;
        float y = 0;

        if (cubes.Length > 0)
        {
            y = cubes.Max(c => c.transform.position.y);
        }

        return y + offsetY + .6f;
    }

    public void UpdateSpeed(float speed)
    {
        float time = timeTakenDuringLerp;

        if (speed <= 100 && speed >= 0)
        {
            time = (100f - speed) / 100f;
            text.text = "Speed of the robot arm has been changed to: " + time + " seconds.";
            speedText.text = "Speed: " + time + " seconds";

            SetSpeedMeter(time);

            timeTakenDuringLerp = time;
        } else
        {
            text.text = "Speed can't go lower than 0 or higher than 100.";
        }
    }
    public void SetSpeedMeter(float time)
    {
        startAngle = (!float.IsNaN(currentAngle)) ? currentAngle : meterPointer.transform.eulerAngles.x;
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

        meterPointer.transform.rotation = Quaternion.Euler(0, 0, currentAngle);

        if ((left && currentAngle >= targetAngle) || (!left && currentAngle <= targetAngle))
        {
            meterPointer.transform.rotation = Quaternion.Euler(0, 0, targetAngle);
            rotateNeedle = false;
        }
    }
    public void SetNeedle(float time)
    {
        float a = GetAngle(time);
        currentAngle = a;
        meterPointer.transform.rotation = Quaternion.Euler(0, 0, a);
    }
    public float GetAngle(float time)
    {
        return -(maxAngle - (maxAngle * time));
    }

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
                if (block.transform.parent = cubes.transform)
                {
                    currentlyHolding = false;
                }
            }

            // after grabbing or releasing a block, go up to the mapBoundaryTop.
            float distanceMapBoundary = mapBoundaryTop - robotArm.transform.position.y;
            StartLerping(Vector3.up, distanceMapBoundary);
        }
    }
}