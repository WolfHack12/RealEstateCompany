namespace Models.Abstract
{
    public abstract class BaseRealEstateModel
    {
        public double Price { get; set; }

        protected BaseRealEstateModel()
            : this(0.0)
        { }

        protected BaseRealEstateModel(double price)
        {
            Price = price;
        }
    }
}
