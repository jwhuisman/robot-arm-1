namespace Assets.Models.WorldData
{
    public class SectionCheck
    {
        public SectionCheck()
        {
            Section = 0;
            SectionRight = 0;
            Dir = 0;
            DirRight = 0;
            NeedNew = false;
            BothWays = false;
        }
        public SectionCheck(int section, int dir)
        {
            NeedNew = true;
            BothWays = false;
            Section = section;
            Dir = dir;
        }
        public SectionCheck(int sectionLeft, int dirLeft, int sectionRight, int dirRight)
        {
            NeedNew = true;
            BothWays = true;
            Section = sectionLeft;
            SectionRight = sectionRight;
            Dir = dirLeft;
            DirRight = dirRight;
        }

        public bool BothWays { get; set; }
        public bool NeedNew { get; set; }
        public int Section { get; set; }
        public int Dir { get; set; }
        public int SectionRight { get; set; }
        public int DirRight { get; set; }
    }
}
