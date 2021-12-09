using BLL.Abstract;
using System.Collections.Generic;

namespace BLL
{
    public class OwnerUser : BaseUser
    {
        public const int MAX_ESTAES = 5
            ;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BankAccount { get; set; }
        public List<BaseRealEstate> RealEstatesList { get; set; }

        public OwnerUser()
            : this("Undefined", "Undefined", "Undefined", "Undefined", 0, null)
        { }

        public OwnerUser(string phone, string firstName, string lastName, string bankAccount, int id,
            List<BaseRealEstate> realEstatesList = null)
            : base(phone, id)
        {
            FirstName = firstName;
            LastName = lastName;
            BankAccount = bankAccount;
            RealEstatesList = realEstatesList ?? new List<BaseRealEstate>();
        }
    }
}
