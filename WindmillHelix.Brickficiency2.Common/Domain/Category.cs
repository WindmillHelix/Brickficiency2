namespace WindmillHelix.Brickficiency2.Common.Domain
{
    public class Category
    {
        public Category()
        {
        }

        public Category(int categoryId, string name)
        {
            CategoryId = categoryId;
            Name = name;
        }

        public int CategoryId { get; set; }

        public string Name { get; set; }
    }
}
