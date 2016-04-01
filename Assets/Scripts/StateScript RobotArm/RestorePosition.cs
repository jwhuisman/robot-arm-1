using UnityEngine;
using Assets.Scripts.View;

public class RestorePosition : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _position = animator.transform.position;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<RobotArm>().targetPosition = _position;
    }

    private Vector3 _position;
}
