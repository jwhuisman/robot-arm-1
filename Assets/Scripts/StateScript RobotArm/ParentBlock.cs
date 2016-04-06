using UnityEngine;
using Assets.Scripts.View;

public class ParentBlock : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject block = animator.GetComponentInParent<RobotArm>().block;
        block.transform.parent = animator.transform;

        // According to model: robot-hand
        // position Y is the exact position the block needs to be at
        // when parented underneath its parent. 
        block.transform.localPosition = new Vector3(0, -3.295874f, block.transform.position.z);
    }
}
