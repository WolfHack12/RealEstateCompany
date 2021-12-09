namespace BLL.Abstract
{
    public abstract class BaseUser
    {
        public int Id { get; set; }
        public string Phone { get; set; }

        protected BaseUser()
            : this("Undefined", 0)
        { }

        protected BaseUser(string phone, int id)
        {
            Id = id;
            Phone = phone;
        }
    }
}
