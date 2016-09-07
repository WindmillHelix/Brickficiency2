namespace WindmillHelix.Brickficiency2.Common.Domain
{
    public class ItemType
    {
        public ItemType()
        {
        }

        public ItemType(string itemTypeCode, string name)
        {
            ItemTypeCode = itemTypeCode;
            Name = name;
        }

        public string ItemTypeCode { get; set; }

        public string Name { get; set; }
    }
}
