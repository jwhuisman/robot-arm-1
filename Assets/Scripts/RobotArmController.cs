using UnityEngine;

public class RobotArmController : MonoBehaviour {

    public GameObject robotArm;
    public GameObject cubes;
    public GameObject plane;

    // lerping related
    private bool _isLerping;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float percentageComplete;
    private float _timeStartedLerping;
    private float timeTakenDuringLerp;

    // pick up, put down a block related.
    private bool currentlyHolding;
    private bool goPickUpBlock;
    private bool goPutDownBlock;
    private bool goUpFromPlane;
    
    private float mapBoundaryTop;

    /// Called to begin the linear interpolation
    void StartLerping(Vector3 direction, float spaces)
    {
        if (!_isLerping || percentageComplete >= 1.0f)
        {
            _isLerping = true;
            _timeStartedLerping = Time.time;
            _startPosition = robotArm.transform.position;
            _endPosition = robotArm.transform.position + direction * spaces;
        }
    }

    // Use this for initialization
    void Start ()
    {
        mapBoundaryTop = 5f;
        timeTakenDuringLerp = 2f;
        goPickUpBlock = false;
        goPutDownBlock = false;
        goUpFromPlane = false;
        currentlyHolding = false;
    }
	
	// Update is called once per frame
	void Update ()
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
            float distanceMapBoundary = mapBoundaryTop - robotArm.transform.position.y;
            StartLerping(Vector3.up, distanceMapBoundary);
        }
    }

    // Updates per millisecond
    void FixedUpdate()
    {
        if (_isLerping)
        {
            float timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / timeTakenDuringLerp;

            robotArm.transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);

            if (percentageComplete >= 1.0f)
            {
                percentageComplete = 1.0f;
                _isLerping = false;
            }
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
            if (raycastNearestDownwardObject.transform.name == plane.name)
            {
                // Note: there is nothing to pick up because the robotArm raycasted the plane and the robotArm doesn't hold a block.
                float distanceTillHitPlane = raycastNearestDownwardObject.distance - (robotArm.transform.localScale.y / 2 ) ;
                StartLerping(Vector3.down, distanceTillHitPlane);

                percentageComplete = 0f;
                goUpFromPlane = true;
            }
            else if (instruction && !currentlyHolding)
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

    public void Actions(string message)
    {
        switch (message)
        {
            case ("move right"):
                Debug.Log("Move Right!");
                StartLerping(Vector3.right, 1);
                break;

            case ("move left"):
                Debug.Log("Move Left!");
                StartLerping(Vector3.left, 1);
                break;

            case ("pick up"):
                Debug.Log("Pick Up!");
                StartPickUpPutDown(true);
                break;

            case ("put down"):
                Debug.Log("Put Down!");
                StartPickUpPutDown(false);
                break;

            default:
                break;
        }
    }
}
