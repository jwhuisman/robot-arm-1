﻿using Assets.Scripts.WorldData;
using System;
using UnityEngine;

namespace Assets.Scripts.View
{
    public class RobotArm : MonoBehaviour
    {
        public GameObject blockDisposal;
        public GameObject blockHolder;
        public Transform robotArmHolder;

        [Space(20)]
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
        [Range(0,100)]
        public int _originalSpeed;

        [Header("Time")]
        public float timeBetweenUpdate;

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

            // Defining/Calculating offsets, positions and transforms
            blockHalf = blockHeight / 2;
            robotArmHolder = transform.parent.transform;


            // At the start of the program the speed starts at 50%
            UpdateSpeed(50);

            _startTime = Time.fixedDeltaTime;

            // Set the height
            UpdateRobotHeight(true);
        }

        public void FixedUpdate()
        {
            if (_originalSpeed == 100)
            {
                // Updates the view on the highest speed, after a certaint given number.(timeBetweenUpdate)
                _counterTime = _startTime + _counterTime + Time.fixedDeltaTime;
                if (_counterTime >= timeBetweenUpdate)
                {
                    MaxSpeedUpdates();

                    _counterTime = 0;
                }
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
            // Prevents the developer to set a value below a certaint point
            // to avoid bugs or illogical numbers.
            distanceToHighestStack = (distanceToHighestStack <= 1) ? 2 : distanceToHighestStack;
            timeBetweenUpdate = (timeBetweenUpdate <= 0.01 && timeBetweenUpdate >= 60) ? 1 : timeBetweenUpdate;

            if (_animator != null && _originalSpeed != _animator.GetInteger("Speed"))
            {
                UpdateSpeed(_originalSpeed);
            }

        }

        public void UpdateRobotHeight(bool set)
        {
            if (_world == null)
            {
                // We're in the editor, or the robot arm hasn't been fully initialized
                // yet, so we can ignore this call.
                return;
            }

            // Height changes
            if (_world.Height != _worldHeight)
            {
                hangingHeight = (_world.Height * blockHeight) + robotArmHeight + distanceToHighestStack;
                _worldHeight = _world.Height;
            }

            if (set)
            {
                // sets the object on the correct Y position
                robotArmHolder.position = new Vector3(robotArmHolder.position.x, hangingHeight);
            }
        }

        public void MoveLeft()
        {
            // Calculates with the width and spacing between blocks.
            // So that we stand above the next block to the left.
            targetPosition = new Vector3(targetPosition.x - (blockWidth * _view.spacing), hangingHeight);

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
            targetPosition = new Vector3(targetPosition.x - (-blockWidth * _view.spacing), hangingHeight);

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
        }

        public void Scan()
        {
            if (_world.RobotArm.Holding && _originalSpeed < 100)
            {
                _animator.SetTrigger("Scan animation");
            }
            else
            {
                OnAnimationIsDone();
            }
        }

        public void MaxSpeedUpdates()
        {
            _sectionBuilder.ReloadSectionsAtCurrent();

            UpdateRobotHeight(false);

            // sets the object on the correct X and Y position
            robotArmHolder.position = new Vector3(targetPosition.x, hangingHeight);

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
            float animatorSpeed = curveNormalized * 250;

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

        internal float hangingHeight;

        private float _startTime;
        private float _counterTime;
        private float _worldHeight;

        private SectionBuilder _sectionBuilder;
        private Animator _animator;
        private World _world;
        private View _view;
    }
}