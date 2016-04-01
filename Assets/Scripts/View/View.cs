using Assets.Scripts.WorldData;
using UnityEngine;

namespace Assets.Scripts.View
{
    public class View : MonoBehaviour
    {
        public float spacing = 1.25f;

        // stuff the view knows
        public SectionBuilder sectionBuilder;
        public GameObject robotArm;
        public GameObject speedMeter;

        // start
        public void Start()
        {
            InitComponents();

            InitSectionSize();
        }

        // update
        public void UpdateView()
        {
            sectionBuilder.CheckSectionsToCreate();
            sectionBuilder.CheckSectionsToRender();
        }

        // Initialize
        public void InitComponents()
        {
            _globals = GameObject.Find("Global Scripts").GetComponent<Globals>();
            _view = GameObject.Find("View");
            _world = _globals.world;

            _robotArmData = _world.RobotArm;
            sectionBuilder = _view.GetComponent<SectionBuilder>();
        }
        public void InitSectionSize()
        {
            sectionWidthTotal = (int)sectionBuilder.assemblyLineModel.GetComponent<MeshRenderer>().bounds.size.x;
            sectionWidth = sectionWidthTotal - (sectionWidthTotal / 4);
            spacing = (float)sectionWidthTotal / (float)sectionWidth;

            sectionBuilder.SetSectionSize(sectionWidthTotal, sectionWidth, spacing);
        }
        
        // helpers
        public GameObject FindBlock(string Id)
        {
            return GameObject.Find("Block-" + Id);
        }
        public float WorldToView(int x)
        {
            return x * spacing;
        }

        // privates
        private int sectionWidthTotal;
        private int sectionWidth;

        private bool wasHolding = false;

        private GameObject     _view;
        private RobotArmData   _robotArmData;
        private Globals        _globals;
        private World          _world;
    }
}