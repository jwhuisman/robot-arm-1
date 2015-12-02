﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class RobotArmController : MonoBehaviour {

    public GameObject arm;
    public Text text;

    private float timeTakenDuringLerp = 1f;
    private bool _isLerping;
    private float percentageComplete;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _timeStartedLerping;

    private bool hold = false;
    private string holding = null;

    /// Called to begin the linear interpolation
    void StartLerping(Vector3 direction, float spaces)
    {
        if (!_isLerping || percentageComplete >= 1.0f)
        {
            _isLerping = true;
            _timeStartedLerping = Time.time;
            _startPosition = arm.transform.position;
            _endPosition = arm.transform.position + direction * spaces;
            print("The end position is :" + _endPosition);

        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    // Updates per millisecond
    void FixedUpdate()
    {
        if (_isLerping)
        {
            float timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / timeTakenDuringLerp;

            arm.transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);

            if (percentageComplete >= 1.0f)
            {
                _isLerping = false;
            }
        }
    }

    public void Actions (string message)
    {
        string[] command = message.Split(' ');

        switch (command[0])
        {
            case ("move"):
                if (command[1] == "right")
                {
                    StartLerping(Vector3.right, 1);
                    text.text = "Moving to the right.";
                }
                else if (command[1] == "left")
                {
                    StartLerping(Vector3.left, 1);
                    text.text = "Moving to the left.";
                }
                break;

            case ("speed"):
                int number;
                bool isInt = Int32.TryParse(command[1], out number);
                if (isInt && number <= 100 && number >= 0)
                {
                    float time = (100f - (float)Int32.Parse(command[1])) / 100f;
                    text.text = "Speed of the robot arm has been changed to: " + time;
                    timeTakenDuringLerp = time;
                }
                else
                {
                    text.text = "You can't go lower than 0, or higher than 100.";
                }
                break;

            default:
                break;
        }
    }
}
