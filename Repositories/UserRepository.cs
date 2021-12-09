using DAL;
using DAL.Abstract;
using Entities;
using Entities.Abstract;
using Exceptions;
using Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        public string Path { get; set; }
        public Type[] SubclassTypes { get; set; }
        public IDataProvider DataProvider { get; set; }

        private static List<BaseUserEntity> collection;

        public UserRepository()
        {
            Path = @".\users.dat";
            SubclassTypes = new Type[]
            {
                typeof(BaseUserEntity),
                typeof(OwnerUserEntity),
            };
            DataProvider = new JSONDataProvider();
            collection = Refresh();
        }

        public List<BaseUserEntity> GetList()
        {
            collection = Refresh();
            return collection;
        }

        public void Add(BaseUserEntity user)
        {
            collection.Add(user);
            Save();
        }

        public void Update(List<BaseUserEntity> users)
        {
            DataProvider.Delete(Path);
            collection = users;
            Save();
        }

        private void Save()
        {
            DataProvider.Write(Path, collection, typeof(List<BaseUserEntity>), SubclassTypes);
        }

        private List<BaseUserEntity> Refresh()
        {
            List<BaseUserEntity> users;

            try
            {
                users = DataProvider
                    .Read(Path, typeof(List<BaseUserEntity>), SubclassTypes)
                    as List<BaseUserEntity>;
            }
            catch (SerializationException)
            {
                users = null;
            }
            catch (InvalidOperationException)
            {
                users = null;
            }

            return users ?? new List<BaseUserEntity>();
        }

        public BaseUserEntity FindByPhone(string phone)
        {
            collection = Refresh();
            return collection.Find(user => user.Phone == phone);
        }

        public void Update(BaseUserEntity data)
        {
            collection = Refresh();
            var updated = collection.FindIndex(user => user.Id == data.Id);
            collection[updated] = data;
            DataProvider.Delete(Path);
            Save();
        }

        public void Delete(int id)
        {
            collection = Refresh();
            var idx = collection.FindIndex(user => user.Id == id);
            if (idx < 0)
            {
                throw new UnexistantUserException($"User id: { id } does not exists.");
            }
            collection.RemoveAt(idx);
            Save(); 
        }

        public OwnerUserEntity ExtendedOwnerSearch(string firstName, string lastName, string bankAccount)
        {
            collection = Refresh();
            var searched = collection.Find(user =>
            {
                if (user is OwnerUserEntity owner)
                {
                    return owner.FirstName == firstName && owner.LastName == lastName && owner.BankAccount == bankAccount;
                }
                else
                {
                    return false;
                }
            });
            if (searched != null)
            {
                return searched as OwnerUserEntity;
            }
            return null;
        }
    }
}
