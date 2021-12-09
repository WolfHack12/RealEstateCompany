using Entities.Abstract;
using System.Collections.Generic;

namespace Entities
{
    public class OwnerUserEntity : BaseUserEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BankAccount { get; set; }
        public List<int> RealEstatesList { get; set; }

        public OwnerUserEntity() 
            : this("Undefined", "Undefined", "Undefined", "Undefined", 0, null)
        { }

        public OwnerUserEntity(string phone, string firstName, string lastName, string bankAccount, int id,
            List<int> realEstatesList = null)
            : base(phone, id)
        {
            FirstName = firstName;
            LastName = lastName;
            BankAccount = bankAccount;
            RealEstatesList = realEstatesList ?? new List<int>();
        }
    }
}
