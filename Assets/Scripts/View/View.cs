﻿using Assets.Models.WorldData;
using Assets.Scripts.WorldData;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.View
{
    public class View : MonoBehaviour
    {
        // blender models
        public GameObject robotArmModel;

        // stuff the view knows
        public SpeedMeter speedMeter;
        public SectionBuilder sectionBuilder;


        // start
        public void Start()
        {
            InitObjects();

            InitRobotArm();

            InitSectionSize();
        }

        // update
        public void UpdateView()
        {
            UpdateWorld();

            sectionBuilder.CheckSectionsToRender();
            sectionBuilder.CheckSections();
        }
        public void UpdateWorld()
        {
            _world = _globals.world;

            _robotArm.transform.position = RobotArmToView(_robotArmData);


            if (_robotArmData.Holding)
            {
                FindBlock(_robotArmData.HoldingBlock.Id).transform.position = new Vector3(_robotArm.transform.position.x, _robotArm.transform.position.y - .6f);
                wasHolding = true;
            }
            else if (wasHolding)
            {
                int i = _world.Stacks.FindIndex(s => s.Id == _robotArmData.X);
                float y = _world.Stacks[i].Blocks.Count() - 1;

                FindBlock(_robotArmData.HoldingBlock.Id).transform.position = new Vector3(_robotArm.transform.position.x, y);
                _robotArmData.HoldingBlock = new Block();

                wasHolding = false;
            }
        }
        
        // initialize
        public void InitObjects()
        {
            _globals = GameObject.Find("Globals").GetComponent<Globals>();
            _view = GameObject.Find("View");

            _cubes = GameObject.Find("Cubes");
            _world = _globals.world;
            _robotArmData = _world.RobotArm;

            speedMeter = _view.GetComponent<SpeedMeter>();
            sectionBuilder = _view.GetComponent<SectionBuilder>();
        }
        public void InitRobotArm()
        {
            CreateRobotArm(_robotArmData);
        }

        // create
        public void CreateRobotArm(RobotArmData robotArmData)
        {
            GameObject arm = Instantiate(robotArmModel);

            arm.name = "RobotArm";
            arm.tag  = "RobotArm";
            arm.transform.parent = _view.transform;
            arm.transform.position = new Vector3(robotArmData.X, robotArmData.Y, 0);
            arm.transform.rotation = Quaternion.identity;

            _robotArm = arm;
        }

        // helpers
        public GameObject FindBlock(string Id)
        {
            return GameObject.Find("Block-" + Id);
        }
        public GameObject FindBlockAtX(int x)
        {
            GameObject[] stack = GameObject.FindGameObjectsWithTag("Block").Where(b => b.transform.position.x == WorldToView(x)).ToArray();

            if (stack.Count() != 0)
            {
                float  y = stack.Max(s => s.transform.position.y);
                return stack.Where(s => s.transform.position.y == y).SingleOrDefault();
            }

            return null;
        }
        public float WorldToView(int x)
        {
            return x * spacing;
        }
        public Vector3 RobotArmToView(RobotArmData robotArmData)
        {
            float x = robotArmData.X * spacing;
            float y = robotArmData.Y;

            return new Vector3(x, y);
        }

        // misc
        public void InitSectionSize()
        {
            sectionWidthTotal = (int)sectionBuilder.assemblyLineModel.GetComponent<MeshRenderer>().bounds.size.x;
            sectionWidth = sectionWidthTotal - (sectionWidthTotal / 4);
            spacing = (float)sectionWidthTotal / (float)sectionWidth;

            sectionBuilder.SetSectionSize(sectionWidthTotal, sectionWidth, spacing);
        }


        // privates
        private List<int> instantiatedStacks = new List<int>();
        private int sectionWidthTotal;
        private int sectionWidth;
        private float spacing;

        private Material[] oldMaterials = new Material[2];
        private GameObject lastBlock;
        private bool wasHolding = false;

        private GameObject _view;
        private GameObject _cubes;
        private GameObject _factory;
        private GameObject _robotArm;
        private RobotArmData   _robotArmData;
        private Globals    _globals;
        private World      _world;
    }
}