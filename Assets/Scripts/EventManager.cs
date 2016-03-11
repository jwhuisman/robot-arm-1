using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{

    public delegate void AnimationAction();

    public static event AnimationAction AnimationIsDone;

    void Start()
    {
        _robotArmAnimator = GameObject.FindWithTag("RobotArm").GetComponent<Animator>();
    }

    void Update()
    {
        // checks if an animation is finished
        if (_robotArmAnimator.GetBool("AnimationIsDone"))
        {
            AnimationFinished();
            _robotArmAnimator.SetBool("AnimationIsDone", false);
        }
    }

    public static void AnimationFinished()
    {
        if (AnimationIsDone != null)
            AnimationIsDone();
    }

    private Animator _robotArmAnimator;
}
