using Entities.Abstract;
using System.Collections.Generic;

namespace Repositories.Abstract
{
    public interface IRealEstateRepository
    {
        List<BaseRealEstateEntity> GetList();

        void Add(BaseRealEstateEntity data);

        void Update(List<BaseRealEstateEntity> data);

        BaseRealEstateEntity FindById(int id);
    }
}
