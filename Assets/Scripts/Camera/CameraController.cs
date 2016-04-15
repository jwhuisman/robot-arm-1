using Assets.Scripts.View;
using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public RobotArm robotArmScript;

    void Start()
    {
        perspectiveSwitcher = GetComponent<PerspectiveSwitcher>();
        robotArm = GameObject.Find(Tags.RobotArm);
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

        if (!perspectiveSwitcher.orthoOn) {
            newCam.y = (newCam.z < camStartZ) ? camStartY + yDiff : camStartY;
        } else {
            newCam.y = robotArm.transform.position.y / 2 + 1f;
        }

        if (perspectiveSwitcher.orthoOn) {
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, perspectiveSwitcher.GetNewOrthoSize(), 1f * Time.deltaTime);
        }

        transform.position = Vector3.Lerp(cam, newCam, smooth * Time.deltaTime);
    }


    private PerspectiveSwitcher perspectiveSwitcher;
    private GameObject robotArm;
    private float roboStartY;
    private float camStartY;
    private float camStartZ;
    private float smooth = 5f; // the higher this is, the less likely it is for sections not te render/create
}