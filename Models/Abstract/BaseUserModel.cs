namespace Models.Abstract
{
    public abstract class BaseUserModel
    {
        public string Phone { get; set; }

        protected BaseUserModel()
            : this("Undefined")
        { }

        protected BaseUserModel(string phone)
        {
            Phone = phone;
        }
    }
}
