using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject robotArm;

    private float smooth = .5f;

    private float armStartY;
    private float camStartY;
    private float camStartZ;
    private float vertDistance;

    void Start ()
    {
        robotArm = GameObject.FindGameObjectWithTag("RobotArm");
        armStartY = robotArm.transform.position.y;
        camStartY = transform.position.y;
        camStartZ = transform.position.z;
        vertDistance = armStartY - camStartY;
    }
	
	void FixedUpdate ()
    {
        Vector3 robotArmPos = robotArm.transform.position;
        Vector3 camPos = transform.position;
        Vector3 newCamPos = camPos;

        newCamPos.x = robotArmPos.x;
        newCamPos.y = (robotArmPos.y > armStartY) ? robotArmPos.y - vertDistance : camStartY;
        newCamPos.z = (robotArmPos.y > 6.6f) ? (-15) - (newCamPos.y) : camStartZ; 

        transform.position = Vector3.Lerp(camPos, newCamPos, smooth * Time.deltaTime);
    }
}