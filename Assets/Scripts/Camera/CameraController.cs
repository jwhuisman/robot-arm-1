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

        if (!perspectiveSwitcher.orthoOn) {
            newCam.y = (newCam.z < camStartZ) ? camStartY + yDiff : camStartY;
            newCam.z = (armPos.y > roboStartY) ? camStartZ - zDiff : camStartZ;
        } else
        {
            newCam.y = robotArm.transform.position.y / 2 + 1f;
            GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, perspectiveSwitcher.GetNewOrthoSize(), 1f * Time.deltaTime);
        }

        transform.position = Vector3.Lerp(cam, newCam, smooth * Time.deltaTime);
    }

    public void ParentCamera(bool makeCameraChild)
    {
        if (makeCameraChild)
        {
            transform.SetParent(GameObject.FindGameObjectWithTag(Tags.RobotHolder).transform);
            //GetComponent<CameraController>().enabled = false;
        }
        else
        {
            transform.SetParent(null);
            //GetComponent<CameraController>().enabled = true;
        }
    }

    private PerspectiveSwitcher perspectiveSwitcher;
    private GameObject robotArm;
    private float roboStartY;
    private float camStartY;
    private float camStartZ;
    private float smooth = 5f; // the higher this is, the less likely it is for sections not te render/create
}