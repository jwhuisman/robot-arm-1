using UnityEngine;

namespace Assets.Scripts.View
{
    public class SpeedMeter : MonoBehaviour
    {
        [Range(0, 1)]
        public float position;
        
        [Range(0, 1)]
        public float target = 0.5f;

        public float minAngle = 110f;
        public float maxAngle = -110f;

        public float rotationSpeed = 1f;

        public void Update()
        {
            UpdateNeedle();
        }
        
        public void UpdateNeedle()
        {
            // Moves the needle
            position = Mathf.MoveTowards(position, target, rotationSpeed * Time.deltaTime);

            // Checks the current position between the to vectors
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.z = Mathf.Lerp(minAngle, maxAngle, position);

            // Implement the rotation back to its transform
            transform.rotation = Quaternion.Euler(rotation);
        }

        public void UpdateSpeed(int speed)
        {
            target = speed / 100f;
        }
        
        //public void UpdateNeedle()
        //{
        //    float curSpeed = GetAngle(GameObject.Find("robot-hand").GetComponent<Animator>().GetFloat("Speed") / 30);

        //    if (armSpeed != curSpeed)
        //    {
        //        // Rotate 
        //        RotateNeedleTowards(GetAngle(curSpeed));
        //    }
        //}

        //public void RotateNeedleTowards(float toAngle)
        //{
        //    Vector3 tarAngle = new Vector3(0, 0, toAngle);
        //    Vector3 newDir = Vector3.RotateTowards(transform.forward, tarAngle, rotSpeed, 0.0F);
        //    transform.rotation = Quaternion.LookRotation(newDir);
        //}


        //public void UpdateNeedle()
        //{
        //    float curSpeedAngle = GameObject.Find("robot-hand").GetComponent<Animator>().GetFloat("Speed") / 30;


        //    //if (armSpeed != currentSpeed)
        //    //{
        //    //    if (!rotateNeedle)
        //    //    {
        //    //        SetSpeed(currentSpeed);
        //    //    }
        //    //    else if (rotateNeedle)
        //    //    {
        //    //        RotateNeedleTowards();
        //    //    }
        //    //}
        //}


        //public void SetSpeed(float time)
        //{
        //    armSpeed = time;

        //    SetSpeedMeter(time);
        ////}
        //public void SetSpeedMeter(float speed)
        //{
        //    startAngle = (!float.IsNaN(currentAngle)) ? currentAngle : needle.transform.eulerAngles.x;
        //    currentAngle = startAngle;
        //    targetAngle = GetAngle(speed);

        //    if (targetAngle != startAngle)
        //    {
        //        rotateNeedle = true;
        //    }
        ////}
        //public void RotateNeedleTowards()
        //{
        //    float speed = rotSpeed * (Math.Abs(targetAngle - startAngle) / 40);
        //    bool left = (startAngle < targetAngle) ? true : false;

        //    currentAngle = (left) ? currentAngle + speed : currentAngle - speed;

        //    needle.transform.rotation = Quaternion.Euler(0, 0, currentAngle);

        //    if ((left && currentAngle >= targetAngle) || (!left && currentAngle <= targetAngle))
        //    {
        //        needle.transform.rotation = Quaternion.Euler(0, 0, targetAngle);
        //        rotateNeedle = false;
        //    }
        //}


        //public void CreateSpeedMeter()
        //{
        //    GameObject meter = Instantiate(speedMeterModel);
        //    meter.name = "SpeedMeter";
        //    meter.tag = "SpeedMeter";
        //    needle = GameObject.Find("Needle");

        //    Transform parentTrans = GameObject.Find("Robot").transform;
        //    meter.transform.parent = parentTrans;

        //    // reset the rotations, to fix issues with blender -> unity
        //    meter.transform.position = new Vector3(parentTrans.position.x, parentTrans.position.y + 1.5f, parentTrans.position.z);
        //    foreach (Transform child in meter.transform)
        //    {
        //        child.rotation = Quaternion.identity;
        //    }

        //    SetNeedle();
        //}

        //public void SetNeedle()
        //{
        //    float a = GetAngle(GameObject.Find("robot-hand").GetComponent<Animator>().GetFloat("Speed") / 30);
        //    currentAngle = a;
        //    needle.transform.rotation = Quaternion.Euler(0, 0, a);
        //}

        // helper

        //public float GetAngle(float speed)
        //{
        //    return -(maxAngle - (maxAngle * speed));
        //}



        //private GameObject needle;

        //private float armSpeed;
        //private float startAngle;
        //private float currentAngle;
        //private float targetAngle;
        ////private float maxAngle = 240;
        //private float rotSpeed = 3f;
        //private bool rotateNeedle = false;
    }
}
