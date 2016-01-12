using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject robotArm;

    private float smooth = 1.5f;

    private float armStartY;
    private float camStartY;
    private float vertDistance;

    void Start ()
    {
        robotArm = GameObject.FindGameObjectWithTag("RobotArm");
        armStartY = robotArm.transform.position.y;
        camStartY = transform.position.y;
        vertDistance = armStartY - camStartY;
    }
	
	void FixedUpdate ()
    {
        Vector3 robotArmPos = robotArm.transform.position;
        Vector3 camPos = transform.position;
        Vector3 newCamPos = camPos;

        newCamPos.x = robotArmPos.x;
        newCamPos.y = (robotArmPos.y > armStartY) ? robotArmPos.y - vertDistance : newCamPos.y = camStartY;

        transform.position = Vector3.Lerp(camPos, newCamPos, smooth * Time.deltaTime);
    }
}