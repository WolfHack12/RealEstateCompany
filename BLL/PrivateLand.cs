using BLL.Abstract;

namespace BLL
{
    public class PrivateLand : BaseRealEstate
    {
        public double LandArea { get; set; }

        public PrivateLand()
            : this(0.0, 0.0, 0)
        { }

        public PrivateLand(double landArea, double price, int id)
            : base(price, id)
        {
            LandArea = landArea;
        }
    }
}
