namespace Assets.Models.World
{
    public class SectionCheck
    {
        public SectionCheck()
        {
            Dir = 0;
            NeedNew = false;
        }

        public bool NeedNew { get; set; }
        public int Dir { get; set; }
    }
}
