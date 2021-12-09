using BLL;
using BLL.Abstract;
using Entities;
using Exceptions;
using Mappers;
using Repositories;
using Repositories.Abstract;
using Services.Abstract;
using System;
using System.Collections.Generic;

namespace Services
{
    public class RealEstateService : IRealEstateService
    {
        private IRealEstateRepository realEstateRepository = new RealEstateRepository();
        private IUserRepository userRepository = new UserRepository();

        private void AddToOwner(int id, string ownerPhone)
        {
            var user = userRepository.FindByPhone(ownerPhone) as OwnerUserEntity;
            if (user == null)
            {
                throw new UnexistantUserException($"User { ownerPhone } does not exist.");
            }
            if (user.RealEstatesList.Count >= OwnerUser.MAX_ESTAES)
            {
                throw new OverflowEstatesCollectionException($"User { ownerPhone } reached estates limit ({ OwnerUser.MAX_ESTAES }).");
            }
            user.RealEstatesList.Add(id);
            userRepository.Update(user);
        }

        public void Add(PrivateLand privateLand, string ownerPhone)
        {
            int id = AssignId();
            privateLand.Id = id;
            AddToOwner(id, ownerPhone);

            realEstateRepository.Add(privateLand.ToEntity());
        }

        public void Add(Flat flatModel, string ownerPhone)
        {
            int id = AssignId();
            flatModel.Id = id;
            AddToOwner(id, ownerPhone);

            realEstateRepository.Add(flatModel.ToEntity());
        }

        public void Delete(string ownerPhone, int index)
        {
            var user = userRepository.FindByPhone(ownerPhone) as OwnerUserEntity;
            if (index < 0 || index >= user.RealEstatesList.Count)
            {
                throw new System.IndexOutOfRangeException();
            }
            var reList = realEstateRepository.GetList();
            var reToDelete = reList.Find(re => re.Id == user.RealEstatesList[index]);
            reList.Remove(reToDelete);
            realEstateRepository.Update(reList);

            user.RealEstatesList.RemoveAt(index);
            userRepository.Update(user);
        }

        public void Delete(string ownerPhone)
        {
            var user = userRepository.FindByPhone(ownerPhone) as OwnerUserEntity;
            var reList = realEstateRepository.GetList();
            user.RealEstatesList.ForEach(id => reList.RemoveAt(reList.FindIndex(re => re.Id == id)));
            realEstateRepository.Update(reList);
        }

        public List<BaseRealEstate> GetList()
        {
            return realEstateRepository.GetList().ToDomain();
        }

        private int AssignId()
        {
            int lastId = 0;
            foreach (var item in realEstateRepository.GetList().ToDomain())
            {
                if (item.Id > lastId)
                {
                    lastId = item.Id;
                }
            }
            return lastId + 1;
        }

        public List<BaseRealEstate> SortByType(List<BaseRealEstate> estates)
        {
            var copy = new List<BaseRealEstate>(estates);
            copy.Sort((a, b) =>
            {
                if (a is Flat f1 && b is Flat f2)
                {
                    if (f1.Rooms > f2.Rooms)
                    {
                        return 1;
                    }
                    return -1;
                }
                else
                {
                    if (a is Flat flat)
                    {
                        return -1;
                    }
                    return 1;
                }
            });
            return copy;
        }

        public List<BaseRealEstate> SortByPrice(List<BaseRealEstate> estates)
        {
            var copy = new List<BaseRealEstate>(estates);
            copy.Sort((a, b) => a.Price.CompareTo(b.Price));
            return copy;
        }

        public List<BaseRealEstate> GetOwnerEstates(string phone)
        {
            var owner = new UserService().GetList().Find(user => user.Phone == phone) as OwnerUser;
            if (owner != null)
            {
                return owner.RealEstatesList;
            }
            else
            {
                throw new UnexistantUserException($"User { phone } does not exist.");
            }
        }

        public void EditFlat(string ownerPhone, int index, Flat data)
        {
            var user = userRepository.FindByPhone(ownerPhone) as OwnerUserEntity;
            if (index < 0 || index >= user.RealEstatesList.Count)
            {
                throw new System.IndexOutOfRangeException();
            }
            var reList = realEstateRepository.GetList();
            var idxToEdit = reList.FindIndex(re => re.Id == user.RealEstatesList[index]);
            if (reList[idxToEdit] as FlatEntity == null)
            {
                throw new UnexpectedRealEstateTypeException("");
            }
            data.Id = reList[idxToEdit].Id;
            reList[idxToEdit] = data.ToEntity();
            realEstateRepository.Update(reList);
        }

        public void EditPrivateLand(string ownerPhone, int index, PrivateLand data)
        {
            var user = userRepository.FindByPhone(ownerPhone) as OwnerUserEntity;
            if (index < 0 || index >= user.RealEstatesList.Count)
            {
                throw new System.IndexOutOfRangeException();
            }
            var reList = realEstateRepository.GetList();
            var idxToEdit = reList.FindIndex(re => re.Id == user.RealEstatesList[index]);
            if (reList[idxToEdit] as PrivateLandEntity == null)
            {
                throw new UnexpectedRealEstateTypeException("");
            }
            data.Id = reList[idxToEdit].Id;
            reList[idxToEdit] = data.ToEntity();
            realEstateRepository.Update(reList);
        }

        public List<BaseRealEstate> SearchByKeyWord(string word)
        {
            var found = new List<BaseRealEstate>();
            var data = realEstateRepository.GetList().ToDomain();
            foreach (var re in data)
            {
                bool mathes = DoesMatch(re, word);
                if (mathes)
                {
                    found.Add(re);
                }
            }
            return found;
        }

        private bool DoesMatch<T>(T data, string keyword)
        {
            Type type = data.GetType();
            var props = type.GetProperties();
            bool matches = false;

            foreach (var prop in props)
            {
                object value = prop.GetValue(data);
                if (value.ToString().Contains(keyword))
                {
                    matches = true;
                }
            }
            return matches;
        }
    }
}
