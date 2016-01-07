using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject robotArm;

    private float smooth = 1.5f;

    void Start ()
    {
        robotArm = GameObject.FindGameObjectWithTag("RobotArm");
    }
	
	void FixedUpdate ()
    {
        Vector3 robotArmPos = robotArm.transform.position;
        Vector3 camPos = transform.position;
        Vector3 newCamPos = camPos;

        newCamPos.x = robotArmPos.x;

        transform.position = Vector3.Lerp(camPos, newCamPos, smooth * Time.deltaTime);
    }
}