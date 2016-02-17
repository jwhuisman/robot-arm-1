﻿using UnityEngine;
using System.Collections;

public class GrabCheck : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<RobotArmController>().GrabStateScript();
    }
}
