using Models.Abstract;
using System;
using System.Collections.Generic;

namespace Models
{
    public class OwnerUserModel : BaseUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BankAccount { get; set; }
        public List<BaseRealEstateModel> RealEstatesList { get; set; }

        public OwnerUserModel()
            : this("Undefined", "Undefined", "Undefined", "Undefined", null)
        { }

        public OwnerUserModel(string phone, string firstName, string lastName, string bankAccount, 
            List<BaseRealEstateModel> realEstatesList = null)
            : base(phone)
        {
            FirstName = firstName;
            LastName = lastName;
            BankAccount = bankAccount;
            RealEstatesList = realEstatesList ?? new List<BaseRealEstateModel>();
        }

        public override string ToString()
        {
            string str = "Owner:\n";
            str += $"First name: { FirstName };\n";
            str += $"Last name: { LastName };\n";
            str += $"Bank account: { BankAccount };\n";
            str += $"Phone: { Phone };\n";
            str += "Real estates:\n";
            if (RealEstatesList.Count > 0)
            {
                RealEstatesList.ForEach(estate =>
                {
                    str += estate.ToString();
                    str += "\n";
                });
            }
            else
            {
                str += "Empty list.\n";
            }
            return str;
        }
    }
}
