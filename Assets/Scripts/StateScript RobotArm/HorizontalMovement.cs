using UnityEngine;
using Assets.Scripts.View;

public class HorizontalMovement : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _robotArmHolder = animator.gameObject.transform.parent.transform;

        _target = new Vector3(animator.GetComponent<RobotArm>().targetPosition.x, _robotArmHolder.transform.position.y, _robotArmHolder.transform.position.z);
        
        // the curved speed is multiplyed by the normalizable speed (so in ratio it moves with the same speed)
        _speed = 1f * animator.GetFloat("CurvedSpeed");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _robotArmHolder.position = Vector3.MoveTowards(_robotArmHolder.position, _target, _speed * Time.deltaTime);
        if (_target == _robotArmHolder.position)
        {
            animator.SetTrigger("Next");
        }
    }

    private float _speed;

    private Vector3 _target;
    private Transform _robotArmHolder;
}
