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
            _view = GameObject.Find(Tags.View);

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

        private GameObject     _view;
    }
}