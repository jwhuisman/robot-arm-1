using Assets.Scripts.WorldData;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.View
{
    public class RobotArm : MonoBehaviour
    {
        public GameObject robotArmModel;
        public GameObject cubeDisposal;

        public float blockHeight = 1.0f;
        public float heightOffset;

        public void Start()
        {
            GetHighestCubeY();

            InitializeComponents();

            UpdateArmHeight();
        }

        public void InitializeComponents()
        {
            _globals = GetComponent<Globals>();
            _view = GameObject.Find("View").GetComponent<View>();
            _world = GameObject.Find("Global Scripts").GetComponent<Globals>().world;
            _animator = gameObject.GetComponentInChildren<Animator>();

            robotArmModel = GameObject.Find("robot-hand");
            _roboCubeDisposal = GameObject.Find("RobotArm-CubeDisposal");
        }
        public void UpdateArmHeight()
        {
            float offset = 1;

            var position = transform.position;
            position.y = (_world.Height + offset) * blockHeight + heightOffset;
            transform.position = position;
        }
        public int GetHighestCubeY()
        {
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block").Where(b => b.name != "Block-" + _world.RobotArm.HoldingBlock.Id).ToArray();
            int offsetY = 3;
            int y = 0;

            if (blocks.Length > 0)
            {
                y = (int)blocks.Max(c => c.transform.position.y);
            }

            return y + offsetY;
        }

        public void MoveLeft()
        {
            targetPosition = new Vector3(transform.position.x - (1f * _view.spacing), transform.position.y, transform.position.z);
            _animator.SetTrigger("Move Left");
        }

        public void MoveRight()
        {
            targetPosition = new Vector3(transform.position.x - (-1f * _view.spacing), transform.position.y, transform.position.z);
            _animator.SetTrigger("Move Right");
        }

        public void Grab()
        {
            // Calculate the position the robot hand needs to move to in order to grab the top
            // block. Note that the top block has already been grabbed in the world, so we need
            // to add 1 to the height of the stack to compensate.
            int stackHeight = _world.CurrentStack.Blocks.Count + 1;
            targetPosition = transform.position;
            targetPosition.y = stackHeight * blockHeight + heightOffset;

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
            targetPosition.y = stackHeight * blockHeight + heightOffset;

            // We still know which block to set down, because it hasn't changed since last we
            // called Grab().

            // Start the animation.
            _animator.SetTrigger("Drop");
        }

        public void PretendGrab()
        {
            int stackHeight = _world.CurrentStack.Blocks.Count + 1;
            targetPosition = transform.position;
            targetPosition.y = stackHeight * blockHeight + heightOffset;

            _animator.SetTrigger("Pretend Grab");
        }

        public void PretendDrop()
        {
            int stackHeight = _world.CurrentStack.Blocks.Count + 1;
            targetPosition = transform.position;
            targetPosition.y = stackHeight * blockHeight + heightOffset;

            _animator.SetTrigger("Pretend Drop");
        }
        
        public void Scan()
        {
            bool ReadyScan = (_world.RobotArm.Holding) ? true : false;
            _animator.SetBool("ReadyScan", ReadyScan);
            _animator.SetTrigger("Scan animation");
        }

        public void UpdateSpeed(int speed)
        {
            float time = 0;

            if (speed <= 100 && speed >= 0)
            {
                time = (100f - speed) / 100f;

                armSpeed = time;
            }

            _view.speedMeter.SetSpeed(time);
        }

        // Helpers and converters
        public GameObject FindBlockAtX(int x, bool viewHolding = false)
        {
            GameObject[] stack = (viewHolding)
                ? GameObject.FindGameObjectsWithTag("Block").Where(b => b.transform.position.x == WorldToView(x) && b.name != "Block-" + _world.RobotArm.HoldingBlock.Id).ToArray()
                : GameObject.FindGameObjectsWithTag("Block").Where(b => b.transform.position.x == WorldToView(x)).ToArray();

            if (stack.Count() != 0)
            {
                float y = stack.Max(s => s.transform.position.y);
                return stack.Where(s => s.transform.position.y == y).SingleOrDefault();
            }

            return null;
        }
        public GameObject FindBlock(string Id)
        {
            return GameObject.Find("Block-" + Id);
        }

        public float WorldToView(int x)
        {
            return x * _view.spacing;
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

        private float mapBoundaryTop;
        private float armSpeed;
        private GameObject _roboCubeDisposal;

        private Animator _animator;
        private Globals _globals;
        private World _world;
        private View _view;
    }
}