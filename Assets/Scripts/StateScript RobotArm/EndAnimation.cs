using Assets.Scripts.View;
using UnityEngine;

public class EndAnimation : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<RobotArm>().OnAnimationIsDone();

        animator.SetTrigger("Correct height");
    }
}