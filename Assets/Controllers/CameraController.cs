using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Start ()
    {
        robotArm = GameObject.Find("robot-hand");
        robStartY = robotArm.transform.position.y;
        camStartZ = transform.position.z;
    }
	
	void FixedUpdate ()
    {
        Vector3 robotArmPos = robotArm.transform.position;
        Vector3 camPos = transform.position;
        Vector3 newCamPos = camPos;

        float zDiff = (robotArmPos.y - robStartY) * zSpeed;

        newCamPos.x = robotArmPos.x;
        newCamPos.z = (robotArmPos.y > robStartY) ? camStartZ - zDiff : camStartZ; 

        transform.position = Vector3.Lerp(camPos, newCamPos, smooth * Time.deltaTime);
    }


    private GameObject robotArm;

    private float zSpeed = 2.0f;
    private float smooth = 1.5f;
    private float robStartY;
    private float camStartZ;
}