using Entities.Abstract;

namespace Entities
{
    public class PrivateLandEntity : BaseRealEstateEntity
    {
        public double LandArea { get; set; }

        public PrivateLandEntity()
            : this(0.0, 0.0, 0)
        { }

        public PrivateLandEntity(double landArea, double price, int id)
            : base(price, id)
        {
            LandArea = landArea;
        }
    }
}
