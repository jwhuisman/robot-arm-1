using UnityEngine;
using System.Collections;
using Assets.Scripts.View;

public class CorrectHeight : StateMachineBehaviour {

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _robotArmHolder = animator.gameObject.transform.parent.transform;

        _speed = animator.GetFloat("CurvedSpeed");

        animator.GetComponent<RobotArm>().UpdateRobotHeight();

        if (_robotArmHolder.position.y != animator.GetComponent<RobotArm>().hangingHeight)
        {
            _target = new Vector3(animator.GetComponent<RobotArm>().targetPosition.x, animator.GetComponent<RobotArm>().hangingHeight, animator.GetComponent<RobotArm>().targetPosition.z);
        }
        else
        {
            _target = _robotArmHolder.position;
        }
    }

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
