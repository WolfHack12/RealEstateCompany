using Entities;
using Entities.Abstract;
using System.Collections.Generic;

namespace Repositories.Abstract
{
    public interface IUserRepository
    {
        List<BaseUserEntity> GetList();

        void Add(BaseUserEntity data);

        void Update(List<BaseUserEntity> data);

        void Update(BaseUserEntity data);

        BaseUserEntity FindByPhone(string phone);
        void Delete(int id);

        OwnerUserEntity ExtendedOwnerSearch(string firstName, string lastName, string bankAccount);
    }
}
