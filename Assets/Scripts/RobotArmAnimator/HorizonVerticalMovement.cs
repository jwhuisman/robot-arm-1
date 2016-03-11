using UnityEngine;
using Assets.Scripts.View;

public class HorizonVerticalMovement : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _robotArm = animator.gameObject.transform;
        _target = new Vector3(_robotArm.GetComponent<RobotArm>().targetPosition.x, _robotArm.GetComponent<RobotArm>().targetPosition.y, _robotArm.GetComponent<RobotArm>().targetPosition.z);
        _speed = 2.5f * animator.GetFloat("Speed");
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

    private float _speed;

    private Vector3 _target;
    private Transform _robotArm;
}
