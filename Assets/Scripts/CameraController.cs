using System;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Start()
    {
        robotArm = GameObject.Find("RobotArm");
        roboStartY = robotArm.transform.position.y;
        camStartY = transform.position.y; // 5.15
        camStartZ = transform.position.z; // -13.63
    }
	
	void FixedUpdate()
    {
        Vector3 armPos = robotArm.transform.position;
        Vector3 cam = transform.position;
        Vector3 newCam = cam;

        float zDiff = (armPos.y - roboStartY);
        float yDiff = Math.Abs(cam.z - camStartZ) / 1.8f;

        newCam.x = armPos.x;
        newCam.z = (armPos.y > roboStartY) ? camStartZ - zDiff : camStartZ;
        newCam.y = (newCam.z < camStartZ) ? camStartY + yDiff : camStartY;

        transform.position = Vector3.Lerp(cam, newCam, smooth * Time.deltaTime);
    }


    private GameObject robotArm;
    private float smooth = 1.5f;
    private float roboStartY;
    private float camStartY;
    private float camStartZ;
}