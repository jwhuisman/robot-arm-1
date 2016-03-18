using System;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Start()
    {
        robotArm = GameObject.Find("Robot");
        roboStartY = robotArm.transform.position.y;
        camStartY = transform.position.y;
        camStartZ = transform.position.z;
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
    private float roboStartY;
    private float camStartY;
    private float camStartZ;
    private float smooth = 5f; // the higher this is, the less likely it is for sections not te render/create
}