using BLL;
using Repositories;
using Repositories.Abstract;
using Services.Abstract;
using Mappers;
using BLL.Abstract;
using Exceptions;
using System.Collections.Generic;
using Entities;
using Entities.Abstract;
using System;

namespace Services
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository = new UserRepository();
        private IRealEstateRepository realEstateRepository = new RealEstateRepository();

        public void Add(OwnerUser user)
        {
            bool isUnique = CheckUniquePhone(user.Phone);
            if (!isUnique)
            {
                throw new AddExistantUserException($"User with { user.Phone } already exists.");   
            }
            user.Id = AssignId();
            userRepository.Add(user.ToEntity());
        }

        public List<BaseUser> GetList()
        {
            var users = userRepository.GetList();
            var usersList = userRepository.GetList().ToDomain();
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i] is OwnerUserEntity ownerEntity)
                {
                    CreateOwnerEstatesById(usersList, ownerEntity, i);
                }
            }
            return usersList;
        }

        private void CreateOwnerEstatesById(List<BaseUser> usersList, 
            OwnerUserEntity ownerEntity, int i)
        {
            usersList.RemoveAt(i);
            var owner = ownerEntity.ToDomain();
            for (int j = 0; j < ownerEntity.RealEstatesList.Count; j++)
            {
                var searched = realEstateRepository.FindById(ownerEntity.RealEstatesList[j]);
                if (searched != null)
                {
                    AddEstate(searched, owner);
                }
            }
            usersList.Insert(i, owner);
        }

        private void AddEstate(in BaseRealEstateEntity searched, in OwnerUser owner)
        {
            if (searched is PrivateLandEntity privateLand)
            {
                owner.RealEstatesList.Add(privateLand.ToDomain());
            }
            else if (searched is FlatEntity flat)
            {
                owner.RealEstatesList.Add(flat.ToDomain());
            }
        }

        public bool IsExist(string phone)
        {
            var users = userRepository.GetList().ToDomain();
            BaseUser existent = users.Find(user => user.Phone == phone);
            if (existent != null)
            {
                return true;
            }
            return false;
        }

        private bool CheckUniquePhone(string phone)
        {
            var users = userRepository.GetList().ToDomain();
            int idx = users.FindIndex(u => u.Phone == phone);
            if (idx > -1)
            {
                return false;
            }
            return true;
        }

        private int AssignId()
        {
            int lastId = 0;
            foreach (var item in userRepository.GetList())
            {
                if (item.Id > lastId)
                {
                    lastId = item.Id;
                }
            }
            return lastId + 1;
        }

        public List<BaseUser> SortOwnersByFirstName(List<BaseUser> users)
        {
            var copy = new List<BaseUser>(users);
            copy.Sort((a, b) => string.Compare((a as OwnerUser).FirstName, (b as OwnerUser).FirstName));
            return copy;
        }

        public List<BaseUser> SortOwnersByLastName(List<BaseUser> users)
        {
            var copy = new List<BaseUser>(users);
            copy.Sort((a, b) => string.Compare((a as OwnerUser).LastName, (b as OwnerUser).LastName));
            return copy;
        }

        public List<BaseUser> SortOwnersByBankAccount(List<BaseUser> users)
        {
            var copy = new List<BaseUser>(users);
            copy.Sort((a, b) => string.Compare((a as OwnerUser).BankAccount, (b as OwnerUser).BankAccount));
            return copy;
        }

        public void ChangeOwner(OwnerUser owner, string oldPhone)
        {
            var user = userRepository.FindByPhone(oldPhone) as OwnerUserEntity;
            if (user == null)
            {
                throw new UnexistantUserException($"User { owner.Phone } does not exist.");
            }
            var ownerEntity = owner.ToEntity();
            ownerEntity.Id = user.Id;
            ownerEntity.RealEstatesList.AddRange(user.RealEstatesList);
            userRepository.Update(ownerEntity);
        }

        public void DeleteOwner(string phone)
        {
            var user = userRepository.FindByPhone(phone) as OwnerUserEntity;
            if (user == null)
            {
                throw new UnexistantUserException($"User { phone} does not exist.");
            }
            new RealEstateService().Delete(phone);
            userRepository.Delete(user.Id);
        }

        public List<BaseUser> FindUsersByKeyword(string word)
        {
            var found = new List<BaseUser>();
            var users = userRepository.GetList();
            var usersList = userRepository.GetList().ToDomain();
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i] is OwnerUserEntity ownerEntity)
                {
                    CreateOwnerEstatesById(usersList, ownerEntity, i);
                }
            }
            foreach (var user in usersList)
            {
                if (user is OwnerUser owner)
                {
                    bool mathes = DoesMatch(owner, word);
                    if (mathes)
                    {
                        found.Add(user);
                    }
                }
            }
            return found;
        }

        private bool DoesMatch(OwnerUser user, string keyword)
        {
            Type type = user.GetType();
            var props = type.GetProperties();
            bool matches = false;

            foreach (var prop in props)
            {
                if (prop.GetValue(user) is List<BaseRealEstate> reList)
                {
                    foreach (var re in reList)
                    {
                        if (re is Flat flat)
                        {
                            string str = flat.ToModel().ToString();
                            if (str.Contains(keyword))
                            {
                                matches = true;
                            }
                        }
                        else if (re is PrivateLand privateLand)
                        {
                            string str = privateLand.ToModel().ToString();
                            if (str.Contains(keyword))
                            {
                                matches = true;
                            }
                        }
                    }
                }
                object value = prop.GetValue(user);
                if (value.ToString().Contains(keyword))
                {
                    matches = true;
                }
            }
            return matches;
        }

        public OwnerUser FindOwner(string firstName, string lastName, string bankAccount)
        {
            var ownerEntity = userRepository.ExtendedOwnerSearch(firstName, lastName, bankAccount);
            if (ownerEntity == null)
            {
                return null;
            }
            var owner = ownerEntity.ToDomain();
            for (int j = 0; j < ownerEntity.RealEstatesList.Count; j++)
            {
                var searched = realEstateRepository.FindById(ownerEntity.RealEstatesList[j]);
                if (searched != null)
                {
                    AddEstate(searched, owner);
                }
            }
            return owner;
        }
    }
}
