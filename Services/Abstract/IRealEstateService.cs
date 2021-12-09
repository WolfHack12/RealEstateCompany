using BLL;
using BLL.Abstract;
using System.Collections.Generic;

namespace Services.Abstract
{
    public interface IRealEstateService
    {
        void Add(PrivateLand privateLand, string ownerPhone);

        List<BaseRealEstate> GetList();

        void Add(Flat flatModel, string ownerPhone);

        List<BaseRealEstate> SortByType(List<BaseRealEstate> estates);

        List<BaseRealEstate> SortByPrice(List<BaseRealEstate> estates);

        void Delete(string ownerPhone, int index);

        List<BaseRealEstate> GetOwnerEstates(string phone);

        void Delete(string ownerPhone);

        void EditFlat(string ownerPhone, int index, Flat data);

        void EditPrivateLand(string ownerPhone, int index, PrivateLand data);

        List<BaseRealEstate> SearchByKeyWord(string word);
    }
}
