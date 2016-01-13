using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RobotArmController : MonoBehaviour {

    public GameObject robotArm;
    public GameObject cubes;
    public GameObject plane;

    // for the testers
    public Text text;
    public Text speedText;

    // lerping related
    public float timeTakenDuringLerp;
    private bool _isLerping;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float percentageComplete;
    private float _timeStartedLerping;

    // pick up, put down a block related.
    public bool currentlyHolding;
    private bool goPickUpBlock;
    private bool goPutDownBlock;
    private bool goUpFromPlane;
    
    private float mapBoundaryTop;

    /// Called to begin the linear interpolation
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

    // Use this for initialization
    void Start ()
    {
        mapBoundaryTop = robotArm.transform.position.y;
        timeTakenDuringLerp = 0.5f;
        speedText.text = "Speed: " + timeTakenDuringLerp + " seconds";
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
            StartLerping(Vector3.up, mapBoundaryTop);
        }

    }

    // Updates per millisecond
    void FixedUpdate()
    {
        if (_isLerping)
        {
            float timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / timeTakenDuringLerp;

            if (_endPosition.x >= -9 && _endPosition.x <= 9)
            {
                robotArm.transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);
            }

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
        // call this function after animating/lerping/etc.
        mapBoundaryTop = GetHighestCubeY();
    }
    public float GetHighestCubeY()
    {
        float y = GameObject.FindGameObjectsWithTag("Cube").Max(c => c.transform.position.y);
        float size = 2f; // now the arm is 2 blocks above the highest block
   
        return y + size + .6f;
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
}
