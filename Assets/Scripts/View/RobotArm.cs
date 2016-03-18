using Assets.Scripts.WorldData;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.View
{
    public class RobotArm : MonoBehaviour
    {
        public GameObject cubeDisposal;

        [Header("Scales")]
        public float blockHeight = 1f;
        public float blockWidth = 1f;
        public float robotArmHeight = 1f;
        public float blockHalf;

        // Space between RobotArm and the heighest Block
        // Warning : DO NOT SET BELOW 1.0f or bad things will happen.
        [Header("Never set below 1")]
        public float distanceToHighestStack = 2.0f;

        public void Start()
        {
            InitializeComponents();
        }
        
        public void OnValidate()
        {
            distanceToHighestStack = (distanceToHighestStack <= 1) ? 2 : distanceToHighestStack;

            UpdateRobotHeight();
        }

        public void InitializeComponents()
        {
            // initializations of scripts
            _globals = GetComponent<Globals>();
            _view = GameObject.Find("View").GetComponent<View>();
            _world = GameObject.Find("Global Scripts").GetComponent<Globals>().world;
            _animator = gameObject.GetComponentInChildren<Animator>();

            _roboCubeDisposal = GameObject.Find("RobotArm-CubeDisposal");

            // Defining/Calculating offset and position
            blockHalf = blockHeight / 2;

            UpdateRobotHeight();
        }

        public void UpdateRobotHeight()
        {
            var position = transform.position;
            position.y = (_world.Height * blockHeight) + robotArmHeight + distanceToHighestStack;
            transform.parent.transform.position = position;
        }
        
        public void MoveLeft()
        {
            // Calculates with the width and spacing between blocks
            // how far it needs to travel to the block at the left.
            targetPosition = new Vector3(transform.position.x - (blockWidth * _view.spacing), transform.position.y, transform.position.z);

            _animator.SetTrigger("Move Left");
        }

        public void MoveRight()
        {
            // Calculates with the width and spacing between blocks
            // how far it needs to travel to the block at the right.
            targetPosition = new Vector3(transform.position.x - (-blockWidth * _view.spacing), transform.position.y, transform.position.z);

            _animator.SetTrigger("Move Right");
        }

        public void Grab()
        {
            // Calculate the position the robot hand needs to move to in order to grab the top
            // block. Note that the top block has already been grabbed in the world, so we need
            // to add 1 to the height of the stack to compensate.
            int stackHeight = _world.CurrentStack.Blocks.Count + 1;
            targetPosition = transform.position;
            targetPosition.y = stackHeight * blockHeight + blockHalf;

            // Find the GameObject that represents the block we're going to grab, so we can
            // parent underneath the robot hand later.
            block = FindBlock(_world.RobotArm.HoldingBlock.Id);

            // Start the animation.
            _animator.SetTrigger("Grab");
        }

        public void Drop()
        {
            // Calculate the position the robot hand needs to move to in order to drop the block
            // on the stap. Note that the block has already been dropped in the world, so we need
            // to subtract 1 from the height of the stack to compensate.
            int stackHeight = _world.CurrentStack.Blocks.Count;
            targetPosition = transform.position;
            targetPosition.y = stackHeight * blockHeight + blockHalf;

            // We still know which block to set down, because it hasn't changed since last we
            // called Grab().

            // Start the animation.
            _animator.SetTrigger("Drop");
        }

        public void PretendGrab()
        {
            int stackHeight = _world.CurrentStack.Blocks.Count + 1;
            targetPosition = transform.position;
            targetPosition.y = stackHeight * blockHeight + blockHalf;

            _animator.SetTrigger("Pretend Grab");
        }

        public void PretendDrop()
        {
            int stackHeight = _world.CurrentStack.Blocks.Count + 1;
            targetPosition = transform.position;
            targetPosition.y = stackHeight * blockHeight + blockHalf;

            _animator.SetTrigger("Pretend Drop");
        }
        
        public void Scan()
        {
            if (_world.RobotArm.Holding)
            {
                _animator.SetTrigger("Scan animation");
            }
            else
            {
                OnAnimationIsDone();
            }
        }

        public void UpdateSpeed(int speed)
        {
            float time = 0;

            if (speed <= 100 && speed >= 0)
            {
                time = (100f - speed) / 100f;
            }

            OnAnimationIsDone();
        }

        // Helpers and converters
        public GameObject FindBlock(string Id)
        {
            return GameObject.Find("Block-" + Id);
        }

        public event EventHandler AnimationIsDone;

        public void OnAnimationIsDone()
        {
            var eventHandler = AnimationIsDone;
            if (eventHandler != null)
            {
                AnimationIsDone(this, EventArgs.Empty);
            }

            UpdateRobotHeight();
        }

        internal Vector3 targetPosition;
        internal GameObject block;

        private GameObject _roboCubeDisposal;

        private Animator _animator;
        private Globals _globals;
        private World _world;
        private View _view;
    }
}