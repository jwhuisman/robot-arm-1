namespace Assets.Models.World
{
    public class SectionCheck
    {
        public SectionCheck()
        {
            Section = 0;
            Dir = 0;
            NeedNew = false;
        }

        public bool NeedNew { get; set; }
        public int Section { get; set; }
        public int Dir { get; set; }
    }
}
