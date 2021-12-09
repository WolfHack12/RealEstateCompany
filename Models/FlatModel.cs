using Models.Abstract;

namespace Models
{
    public class FlatModel : BaseRealEstateModel
    {
        public int Rooms { get; set; }
        public int Floor { get; set; }
        public double LivingArea { get; set; }

        public FlatModel()
            : this(0, 0, 0.0, 0.0)
        { }

        public FlatModel(int rooms, int floor, double livingArea, double price)
            : base(price)
        {
            Rooms = rooms;
            Floor = floor;
            LivingArea = livingArea;
        }

        public override string ToString()
        {
            string str = "Flat:\n";
            str += $"Price: { Price };\n";
            str += $"Rooms: { Rooms };\n";
            str += $"Floor: { Floor };\n";
            str += $"Living area: { LivingArea }.";
            return str;
        }
    }
}
