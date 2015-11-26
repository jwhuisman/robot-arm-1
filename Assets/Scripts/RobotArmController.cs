using UnityEngine;
using System.Collections;

public class RobotArmController : MonoBehaviour {

    public GameObject arm;
    public float timeTakenDuringLerp = 2f;
    private bool hold = false;
    private string holding = null;
    private bool _isLerping;
    private float percentageComplete;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _timeStartedLerping;

    /// Called to begin the linear interpolation
    void StartLerping(Vector3 direction, float spaces)
    {
        if (!_isLerping || percentageComplete >= 1.0f)
        {
            _isLerping = true;
            _timeStartedLerping = Time.time;
            _startPosition = arm.transform.position;
            _endPosition = arm.transform.position + direction * spaces;
            print("The end position is :" + _endPosition);

        }
    }

    // Use this for initialization
    void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



    // Updates per millisecond
    void FixedUpdate()
    {
        if (_isLerping)
        {
            float timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / timeTakenDuringLerp;

            arm.transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);

            if (percentageComplete >= 1.0f)
            {
                _isLerping = false;
            }
        }
    }

    public void Actions (string message)
    {
        switch (message)
        {
            case ("move right"):
                Debug.Log("Move Right!");
                StartLerping(Vector3.right, 1);
                break;

            case ("move left"):
                StartLerping(Vector3.left, 1);
                break;

            default:
                break;
        }
    }
}
