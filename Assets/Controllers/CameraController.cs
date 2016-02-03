using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject robotArm;

    private float smooth = .5f;

    private float camStartZ;

    void Start ()
    {
        robotArm = GameObject.FindGameObjectWithTag("RobotArm");
        camStartZ = transform.position.z;
    }
	
	void FixedUpdate ()
    {
        Vector3 robotArmPos = robotArm.transform.position;
        Vector3 camPos = transform.position;
        Vector3 newCamPos = camPos;

        newCamPos.x = robotArmPos.x;
        newCamPos.z = camStartZ - (robotArmPos.y); 

        transform.position = Vector3.Lerp(camPos, newCamPos, smooth * Time.deltaTime);
    }
}