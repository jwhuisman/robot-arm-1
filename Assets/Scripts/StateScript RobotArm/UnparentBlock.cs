using UnityEngine;
using Assets.Scripts.View;

public class UnparentBlock : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject block = animator.GetComponent<RobotArm>().block;

        block.transform.parent = GameObject.Find(Tags.View).GetComponent<SectionBuilder>().GetCurrentSection().transform.Find("Blocks");
    }
}