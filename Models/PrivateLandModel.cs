using Models.Abstract;

namespace Models
{
    public class PrivateLandModel : BaseRealEstateModel
    {
        public double LandArea { get; set; }

        public PrivateLandModel()
            : this(0.0, 0.0)
        { }

        public PrivateLandModel(double landArea, double price)
            : base(price)
        {
            LandArea = landArea;
        }

        public override string ToString()
        {
            string str = "Private land:\n";
            str += $"Price: { Price };\n";
            str += $"Land area: { LandArea }.";
            return str;
        }
    }
}
