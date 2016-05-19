using UnityEngine;
using Assets.Scripts.View;

public class UpdateHeight : StateMachineBehaviour {

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _robotArmHolder = animator.gameObject.transform.parent.transform;
        _robotArm = animator.GetComponent<RobotArm>();

        // update hangingHeight only
        _robotArm.UpdateRobotHeight(false);

        if (_robotArmHolder.position.y != _robotArm.hangingHeight)
        {
            _target = new Vector3(_robotArm.targetPosition.x, _robotArm.hangingHeight);
        }
        else
        {
            _target = _robotArmHolder.position;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _speed = animator.GetFloat("CurvedSpeed");
        
        // move towards the target
        _robotArmHolder.position = Vector3.MoveTowards(_robotArmHolder.position, _target, _speed * Time.deltaTime);

        if (_target == _robotArmHolder.position)
        {
            animator.SetTrigger("Next");
        }
    }

    private float _speed;

    private Vector3 _target;
    private Transform _robotArmHolder;
    private RobotArm _robotArm;
}
