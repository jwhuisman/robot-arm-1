using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject robotArm;

    private float smooth = 1.5f;

    private float robStartY;
    private float camStartZ;

    void Start ()
    {
        robotArm = GameObject.FindGameObjectWithTag("RobotArm");
        robStartY = robotArm.transform.position.y;
        camStartZ = transform.position.z;
    }
	
	void FixedUpdate ()
    {
        Vector3 robotArmPos = robotArm.transform.position;
        Vector3 camPos = transform.position;
        Vector3 newCamPos = camPos;

        newCamPos.x = robotArmPos.x;
        newCamPos.z = camStartZ - ((robotArmPos.y - robStartY) * 2); 

        transform.position = Vector3.Lerp(camPos, newCamPos, smooth * Time.deltaTime);
    }
}