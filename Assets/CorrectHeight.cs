using UnityEngine;
using System.Collections;
using Assets.Scripts.View;

public class CorrectHeight : StateMachineBehaviour {

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _robotArmHolder = animator.gameObject.transform.parent.transform;

        _target = new Vector3(_robotArmHolder.position.x, animator.GetComponent<RobotArm>().hangingHeight, _robotArmHolder.position.z);
        
        _speed = animator.GetFloat("CurvedSpeed");
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
