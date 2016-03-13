using UnityEngine;
using System.Collections;
using Assets.Scripts.View;

public class UnparentBlock : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject block = animator.GetComponent<RobotArm>().block;
        block.transform.parent = animator.GetComponent<RobotArm>().cubeDisposal.transform;
    }
}
