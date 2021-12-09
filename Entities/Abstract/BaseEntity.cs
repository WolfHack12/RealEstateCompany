namespace Entities.Abstract
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        protected BaseEntity() 
            : this(0)
        { }

        protected BaseEntity(int id)
        {
            Id = id;
        }
    }
}
