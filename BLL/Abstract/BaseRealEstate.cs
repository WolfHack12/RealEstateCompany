namespace BLL.Abstract
{
    public abstract class BaseRealEstate
    {
        public int Id { get; set; }
        public double Price { get; set; }

        protected BaseRealEstate()
            : this(0.0, 0)
        { }

        protected BaseRealEstate(double price, int id)
        {
            Price = price;
            Id = id;
        }
    }
}
