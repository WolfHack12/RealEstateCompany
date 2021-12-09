namespace Entities.Abstract
{
    public abstract class BaseUserEntity : BaseEntity
    {
        public string Phone { get; set; }

        protected BaseUserEntity() 
            : this("Undefined", 0)
        { }

        protected BaseUserEntity(string phone, int id)
            : base(id)
        {
            Phone = phone;
        }
    }
}
