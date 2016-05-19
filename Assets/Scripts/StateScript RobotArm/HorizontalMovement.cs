using UnityEngine;
using Assets.Scripts.View;

public class HorizontalMovement : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _robotArmHolder = animator.gameObject.transform.parent.transform;
        _normalizableSpeed = 2f;

        _target = animator.GetComponent<RobotArm>().targetPosition;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _speed = _normalizableSpeed * animator.GetFloat("CurvedSpeed");

        // move towards the target
        _robotArmHolder.position = Vector3.MoveTowards(_robotArmHolder.position, _target, _speed * Time.deltaTime);
        if (_target == _robotArmHolder.position)
        {
            animator.SetTrigger("Next");
        }
    }

    private float _speed;
    private float _normalizableSpeed;

    private Vector3 _target;
    private Transform _robotArmHolder;
}
