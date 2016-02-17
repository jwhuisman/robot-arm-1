namespace Assets.Models.WorldData
{
    class Section
    {
        public Section(int id, int type)
        {
            Id = id;
            Type = type;
        }

        public int Id { get; set; }
        public int Type { get; set; }
    }
}
