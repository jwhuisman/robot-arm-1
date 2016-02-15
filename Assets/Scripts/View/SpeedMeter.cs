using System;
using UnityEngine;

namespace Assets.Scripts.View
{
    public class SpeedMeter : MonoBehaviour
    {
        public GameObject speedMeterModel;

        public void Start()
        {
            InitNeedle();
        }

        public void InitNeedle()
        {
            CreateSpeedMeter(GameObject.Find("RobotArm").transform);

            needle = GameObject.Find("Needle");

            armSpeed = 0.5f;

            SetNeedle(armSpeed);
        }

        public void Update()
        {
            UpdateNeedle();
        }
        public void UpdateNeedle()
        {
            if (rotateNeedle)
            {
                RotateNeedleTowards();
            }
        }

        public void CreateSpeedMeter(Transform parent)
        {
            GameObject meter = Instantiate(speedMeterModel);
            meter.name = "SpeedMeter";
            meter.tag = "SpeedMeter";
            meter.transform.parent = parent;

            // reset the rotations, to fix issues with blender -> unity
            meter.transform.position = new Vector3(parent.position.x, parent.position.y + 2.5f, 0);
            foreach (Transform child in meter.transform)
            {
                child.rotation = Quaternion.identity;
            }
        }

        public void SetSpeed(float time)
        {
            armSpeed = time;

            SetSpeedMeter(time);
        }
        public void SetSpeedMeter(float speed)
        {
            startAngle = (!float.IsNaN(currentAngle)) ? currentAngle : needle.transform.eulerAngles.x;
            currentAngle = startAngle;
            targetAngle = GetAngle(speed);

            if (targetAngle != startAngle)
            {
                rotateNeedle = true;
            }
        }
        public void RotateNeedleTowards()
        {
            float speed = rotSpeed * (Math.Abs(targetAngle - startAngle) / 40);
            bool left = (startAngle < targetAngle) ? true : false;

            currentAngle = (left) ? currentAngle + speed : currentAngle - speed;

            needle.transform.rotation = Quaternion.Euler(0, 0, currentAngle);

            if ((left && currentAngle >= targetAngle) || (!left && currentAngle <= targetAngle))
            {
                needle.transform.rotation = Quaternion.Euler(0, 0, targetAngle);
                rotateNeedle = false;
            }
        }
        public void SetNeedle(float time)
        {
            float a = GetAngle(time);
            currentAngle = a;
            needle.transform.rotation = Quaternion.Euler(0, 0, a);
        }
        public float GetAngle(float time)
        {
            return -(maxAngle - (maxAngle * time));
        }

        private GameObject needle;
        private float armSpeed;
        private float startAngle;
        private float currentAngle;
        private float targetAngle;
        private float maxAngle = 240;
        private float rotSpeed = 3f;
        private bool rotateNeedle = false;
    }
}
