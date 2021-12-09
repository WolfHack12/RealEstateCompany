using BLL.Abstract;

namespace BLL
{
    public class Flat : BaseRealEstate
    {
        public int Rooms { get; set; }
        public int Floor { get; set; }
        public double LivingArea { get; set; }

        public Flat()
            : this(0, 0, 0.0, 0.0, 0)
        { }

        public Flat(int rooms, int floor, double livingArea, double price, int id)
            : base(price, id)
        {
            Rooms = rooms;
            Floor = floor;
            LivingArea = livingArea;
        }
    }
}
