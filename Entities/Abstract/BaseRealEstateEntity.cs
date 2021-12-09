namespace Entities.Abstract
{
    public abstract class BaseRealEstateEntity : BaseEntity
    {
        public double Price { get; set; }

        protected BaseRealEstateEntity()
            : this(0.0, 0) 
        { }

        protected BaseRealEstateEntity(double price, int id)
            : base(id)
        {
            Price = price;
        }
    }
}
