using Assets.Scripts.View;
using UnityEngine;

public class AnimationIsDone : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // NEEDED animation for this state: Reset.
        animator.SetBool("AnimationIsDone", true);
        animator.SetBool("Pretending", false);
        animator.SetBool("PickUp", false);
        animator.SetBool("PutDown", false);

        animator.GetComponent<RobotArm>().UpdateArmHeight();
    }
}
