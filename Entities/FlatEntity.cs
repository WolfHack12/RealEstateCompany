using Entities.Abstract;

namespace Entities
{
    public class FlatEntity : BaseRealEstateEntity
    {
        public int Rooms { get; set; }
        public int Floor { get; set; }
        public double LivingArea { get; set; }

        public FlatEntity()
            : this(0, 0, 0.0, 0.0, 0)
        { }

        public FlatEntity(int rooms, int floor, double livingArea, double price, int id)
            : base(price, id)
        {
            Rooms = rooms;
            Floor = floor;
            LivingArea = livingArea;
        }
    }
}
