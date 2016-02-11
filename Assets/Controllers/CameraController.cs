using Assets.Scripts;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Start ()
    {
        robotArm = GameObject.Find("RobotArm");
        robStartY = robotArm.transform.position.y;
        camStartZ = transform.position.z;
    }
	
	void FixedUpdate ()
    {
        Vector3 robotArm = this.robotArm.transform.position;
        Vector3 cam = transform.position;
        Vector3 newCam = cam;

        float zDiff = (robotArm.y - robStartY) * zSpeed;

        newCam.x = robotArm.x;
        newCam.z = (robotArm.y > robStartY) ? camStartZ - zDiff : camStartZ;

        transform.position = Vector3.Lerp(cam, newCam, smooth * Time.deltaTime);
    }


    private GameObject robotArm;
    private float zSpeed = 2.0f;
    private float smooth = 1.5f;
    private float robStartY;
    private float camStartZ;
}