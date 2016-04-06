using UnityEngine;
using Assets.Scripts.View;

public class UnparentBlockAndPlace : StateMachineBehaviour {

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject block = animator.GetComponent<RobotArm>().block;
        
        // Calculates the position the block needs to stand 
        _target = animator.GetComponent<RobotArm>().targetPosition;
        _target.y = _target.y - (animator.GetComponent<RobotArm>().blockHalf * 3);

        block.transform.parent = animator.GetComponent<RobotArm>().cubeDisposal.transform;
        block.transform.position = _target;
    }

    private Vector3 _target;
}
