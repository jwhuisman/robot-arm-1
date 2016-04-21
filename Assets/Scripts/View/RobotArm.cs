using Assets.Scripts.WorldData;
using System;
using UnityEngine;

namespace Assets.Scripts.View
{
    public class RobotArm : MonoBehaviour
    {
        public GameObject blockDisposal;
        public GameObject blockHolder;

        [Header("Scales")]
        public float blockHeight = 1f;
        public float blockWidth = 1f;
        public float robotArmHeight = 1f;
        public float blockHalf;

        // Space between RobotArm and the heighest Block
        // Warning : DO NOT SET BELOW 1.0f or bad things will happen.
        [Header("Don't set below 1")]
        public float distanceToHighestStack = 2.0f;

        [Header("Animation curves")]
        public AnimationCurve animationCurveSpeed = new AnimationCurve();


        public void Start()
        {
            InitializeComponents();
        }

        public void InitializeComponents()
        {
            // initializations of scripts
            _view = GameObject.Find(Tags.View).GetComponent<View>();
            _world = GameObject.Find(Tags.Globals).GetComponent<Globals>().world;
            _animator = gameObject.GetComponentInChildren<Animator>();
            _sectionBuilder = _view.GetComponent<SectionBuilder>();

            // Defining/Calculating offset and position
            blockHalf = blockHeight / 2;

            // At the start of the program the speed starts at 50%
            UpdateSpeed(50);

            // At the start of the program the robotarm starts above the highest block
            UpdateRobotHeight();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                MaxSpeedUpdates();
            }
        }

        public int OriginalSpeed
        {
            get
            {
                return _originalSpeed;
            }
            set
            {
                if (value == 100)
                {
                    Camera.main.GetComponent<CameraController>().ParentCamera(true);
                }
                else
                {
                    Camera.main.GetComponent<CameraController>().ParentCamera(false);
                }

                _originalSpeed = value;
            }
        }

        public void OnValidate()
        {
            distanceToHighestStack = (distanceToHighestStack <= 1) ? 2 : distanceToHighestStack;
            //UpdateRobotHeight();
        }

        public void UpdateRobotHeight()
        {
            if (_world == null)
            {
                // We're in the editor, or the robot arm hasn't been fully initialized
                // yet, so we can ignore this call.
                return;
            }

            var position = transform.position;
            position.y = (_world.Height * blockHeight) + robotArmHeight + distanceToHighestStack;
            transform.parent.transform.position = position;
        }

        public void MoveLeft()
        {
            // Calculates with the width and spacing between blocks.
            // So that we stand above the next block to the left.
            targetPosition = new Vector3(targetPosition.x - (blockWidth * _view.spacing), targetPosition.y, targetPosition.z);

            if (_animator.GetInteger("Speed") == 100)
            {
                // When the speed is 100 we want everything 
                // to go instantly so without animations.
                OnAnimationIsDone();
                return;
            }

            // Start the animation.
            _animator.SetTrigger("Move Left");

            _view.UpdateView();
        }

        public void MoveRight()
        {
            // Calculates with the width and spacing between blocks.
            // So that we stand above the next block to the right.
            targetPosition = new Vector3(targetPosition.x - (-blockWidth * _view.spacing), targetPosition.y, targetPosition.z);

            if (_animator.GetInteger("Speed") == 100)
            {
                // When the speed is 100 we want everything 
                // to go instantly so without animations.
                OnAnimationIsDone();
                return;
            }

            // Start the animation.
            _animator.SetTrigger("Move Right");

            _view.UpdateView();
        }

        public void MaxSpeedUpdates()
        {
            // Sets the holders position
            transform.parent.transform.position = new Vector3(targetPosition.x, transform.parent.transform.position.y);
            UpdateRobotHeight();

            _sectionBuilder.ReloadSectionsAtCurrent();
            
            if (!_world.RobotArm.Holding && blockHolder.transform.childCount > 0)
            {
                foreach (Transform child in blockHolder.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            if (_world.RobotArm.Holding)
            {
                if (blockHolder.transform.childCount == 0)
                {
                    // add a block in the robotarm-holder, because the block is in the world.robotArm
                    _sectionBuilder.InstantiateBlock(_world.RobotArm.X, _world.RobotArm.HoldingBlock, true);
                }

                _animator.SetTrigger("MSEP Open");
            }
            else
            {
                _animator.SetTrigger("MSEP Close");
            }

            OnAnimationIsDone();
        }

        public void Grab()
        {
            if (_animator.GetInteger("Speed") == 100)
            {
                // When the speed is 100 we want everything 
                // to go instantly so without animations.
                OnAnimationIsDone();
                return;
            }

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

            _view.UpdateView();
            UpdateRobotHeight();
        }

        public void Drop()
        {
            if (_animator.GetInteger("Speed") == 100)
            {
                // When the speed is 100 we want everything 
                // to go instantly so without animations.
                OnAnimationIsDone();
                return;
            }
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

            _view.UpdateView();
            UpdateRobotHeight();
        }

        public void PretendGrab()
        {
            if (_animator.GetInteger("Speed") == 100)
            {
                // When the speed is 100 we want everything 
                // to go instantly so without animations.
                OnAnimationIsDone();
                return;
            }

            int stackHeight = _world.CurrentStack.Blocks.Count + 1;
            targetPosition = transform.position;
            targetPosition.y = stackHeight * blockHeight + blockHalf;

            _animator.SetTrigger("Pretend Grab");

            _view.UpdateView();
            UpdateRobotHeight();
        }

        public void PretendDrop()
        {
            if (_animator.GetInteger("Speed") == 100)
            {
                // When the speed is 100 we want everything 
                // to go instantly so without animations.
                OnAnimationIsDone();
                return;
            }

            int stackHeight = _world.CurrentStack.Blocks.Count + 1;
            targetPosition = transform.position;
            targetPosition.y = stackHeight * blockHeight + blockHalf;

            _animator.SetTrigger("Pretend Drop");

            _view.UpdateView();
            UpdateRobotHeight();
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
            // Sets the original speed
            _animator.SetInteger("Speed", speed);
            OriginalSpeed = speed;

            // We want to devide our speed by 99, 
            // that will be our maximum animation speed.
            // If speed is 100 the animations shouldn't run
            // and every command goes instantly
            float normalizedSpeed = speed / 99f;

            // float adjusted by the animationCurve
            float curveNormalized = animationCurveSpeed.Evaluate(normalizedSpeed);

            // After the adjustion we multiply to a reasonable range
            // for the animator to multiply the animation states.
            float animatorSpeed = curveNormalized * 30;

            // Update Needle
            _animator.SetFloat("CurvedSpeed", animatorSpeed);

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
        }
        
        internal Vector3 targetPosition;
        internal GameObject block;
        internal int _originalSpeed;

        private SectionBuilder _sectionBuilder;
        private Animator _animator;
        private World _world;
        private View _view;
    }
}