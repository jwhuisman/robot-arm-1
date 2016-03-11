﻿using UnityEngine;
using Assets.Scripts.View;

public class PositionCalculate : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<RobotArm>().PositionCalculateStateScript();
    }
}
