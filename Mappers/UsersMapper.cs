using BLL;
using BLL.Abstract;
using Entities;
using Entities.Abstract;
using Models;
using Models.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace Mappers
{
    public static class UsersMapper
    {
        public static OwnerUserModel ToModel(this OwnerUser domain)
        {
            return new OwnerUserModel
            {
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                BankAccount = domain.BankAccount,
                Phone = domain.Phone,
                RealEstatesList = domain.RealEstatesList.ToModel(),
            };
        }

        public static List<BaseUserModel> ToModel(this List<BaseUser> domains)
        {
            var models = new List<BaseUserModel>();
            foreach (var domain in domains)
            {
                if (domain is OwnerUser owner)
                {
                    models.Add(owner.ToModel());
                }
            }
            return models;
        }

        public static OwnerUser ToDomain(this OwnerUserModel model)
        {
            return new OwnerUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                BankAccount = model.BankAccount,
                Phone = model.Phone,
                RealEstatesList = model.RealEstatesList.ToDomain(),
            };
        }

        public static List<BaseUser> ToDomain(this List<BaseUserModel> models)
        {
            var domains = new List<BaseUser>();
            foreach (var model in models)
            {
                if (model is OwnerUserModel owner)
                {
                    domains.Add(owner.ToDomain());
                }
            }
            return domains;
        }

        public static OwnerUser ToDomain(this OwnerUserEntity entity)
        {
            return new OwnerUser
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                BankAccount = entity.BankAccount,
                Phone = entity.Phone,
                RealEstatesList = new List<BaseRealEstate>(entity.RealEstatesList.Count),
                Id = entity.Id,
            };
        }

        public static List<BaseUser> ToDomain(this List<BaseUserEntity> entities)
        {
            var domains = new List<BaseUser>();
            foreach (var entity in entities)
            {
                if (entity is OwnerUserEntity owner)
                {
                    domains.Add(owner.ToDomain());
                }
            }
            return domains;
        }

        public static OwnerUserEntity ToEntity(this OwnerUser domain)
        {
            return new OwnerUserEntity
            {
                FirstName = domain.FirstName,
                LastName = domain.LastName,
                BankAccount = domain.BankAccount,
                Phone = domain.Phone,
                RealEstatesList = domain.RealEstatesList.ToEntity().Select(re => re.Id).ToList(),
                Id = domain.Id,
            };
        }

        public static List<BaseUserEntity> ToEntity(this List<BaseUser> domains)
        {
            var entities = new List<BaseUserEntity>();
            foreach (var domain in domains)
            {
                if (domain is OwnerUser owner)
                {
                    entities.Add(owner.ToEntity());
                }
            }
            return entities;
        }
    }
}
