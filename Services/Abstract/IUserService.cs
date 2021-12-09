using BLL;
using BLL.Abstract;
using System.Collections.Generic;

namespace Services.Abstract
{
    public interface IUserService
    {
        void Add(OwnerUser user);

        List<BaseUser> GetList();

        bool IsExist(string phone);

        List<BaseUser> SortOwnersByFirstName(List<BaseUser> users);

        List<BaseUser> SortOwnersByLastName(List<BaseUser> users);

        List<BaseUser> SortOwnersByBankAccount(List<BaseUser> users);

        void ChangeOwner(OwnerUser owner, string oldPhone);

        void DeleteOwner(string phone);

        List<BaseUser> FindUsersByKeyword(string word);

        OwnerUser FindOwner(string firstName, string lastName, string bankAccount);
    }
}
