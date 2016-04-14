using UnityEngine;
using Assets.Scripts.View;

public class ParentBlock : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject block = animator.GetComponentInParent<RobotArm>().block;
        block.transform.parent = animator.transform;
    }
}