namespace Wms.ApplicationCore.Entities.MenuAggregate
{
    public class Menu
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string IconPath { get; set; }
        public string Url { get; set; }
        public byte Sequence { get; set; }
        public string Remark { get; set; }
    }
}
