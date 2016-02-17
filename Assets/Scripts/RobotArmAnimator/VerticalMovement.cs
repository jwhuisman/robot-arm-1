using UnityEngine;
using System.Collections;

public class VerticalMovement : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _robotArm = animator.gameObject.transform;
        _target = new Vector3(_robotArm.GetComponent<RobotArmController>().targetPosition.x, _robotArm.GetComponent<RobotArmController>().targetPosition.y, _robotArm.GetComponent<RobotArmController>().targetPosition.z);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _robotArm.position = Vector3.MoveTowards(_robotArm.position, _target, _speed * Time.deltaTime);
        if (_target == _robotArm.position)
        {
            animator.SetTrigger("Next");
        }
    }

    private float _speed = 2f;

    private Vector3 _target;
    private Transform _robotArm;
}
