using UnityEngine;
using Assets.Scripts.View;

public class VerticalMovement : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _robotArm = animator.gameObject.transform;
        _normalizableSpeed = 2f;

        _target = animator.GetComponent<RobotArm>().targetPosition;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
     override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _speed = _normalizableSpeed * animator.GetFloat("CurvedSpeed");
        
        // move towards the target
        _robotArm.position = Vector3.MoveTowards(_robotArm.position, _target, _speed * Time.deltaTime);
        if (_target == _robotArm.position)
        {
            animator.SetTrigger("Next");
        }
    }

    private float _speed;
    private float _normalizableSpeed;

    private Vector3 _target;
    private Transform _robotArm;
}
