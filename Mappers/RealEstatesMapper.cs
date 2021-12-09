using BLL;
using BLL.Abstract;
using Entities;
using Entities.Abstract;
using Models;
using Models.Abstract;
using System.Collections.Generic;

namespace Mappers
{
    public static class RealEstatesMapper
    {
        public static PrivateLandModel ToModel(this PrivateLand domain)
        {
            return new PrivateLandModel
            {
                LandArea = domain.LandArea,
                Price = domain.Price,
            };
        }

        public static FlatModel ToModel(this Flat domain)
        {
            return new FlatModel
            {
                Rooms = domain.Rooms,
                Floor = domain.Floor,
                LivingArea = domain.LivingArea,
                Price = domain.Price,
            };
        }

        public static List<BaseRealEstateModel> ToModel(this List<BaseRealEstate> domains)
        {
            var models = new List<BaseRealEstateModel>();
            foreach (var domain in domains)
            {
                if (domain is PrivateLand privateLand)
                {
                    models.Add(privateLand.ToModel());
                }
                else if (domain is Flat flat)
                {
                    models.Add(flat.ToModel());
                }
            }
            return models;
        }

        public static PrivateLand ToDomain(this PrivateLandModel model)
        {
            return new PrivateLand
            {
                LandArea = model.LandArea,
                Price = model.Price,
                Id = 0,
            };
        }

        public static Flat ToDomain(this FlatModel model)
        {
            return new Flat
            {
                Floor = model.Floor,
                Rooms = model.Rooms,
                LivingArea = model.LivingArea,
                Price = model.Price,
                Id = 0,
            };
        }

        public static List<BaseRealEstate> ToDomain(this List<BaseRealEstateModel> models)
        {
            var domains = new List<BaseRealEstate>();
            foreach (var model in models)
            {
                if (model is PrivateLandModel privateLand)
                {
                    domains.Add(privateLand.ToDomain());
                } 
                else if (model is FlatModel flat)
                {
                    domains.Add(flat.ToDomain());
                }
            }
            return domains;
        }

        public static PrivateLand ToDomain(this PrivateLandEntity entity)
        {
            return new PrivateLand
            {
                Price = entity.Price,
                LandArea = entity.LandArea,
                Id = entity.Id,
            };
        }

        public static Flat ToDomain(this FlatEntity entity)
        {
            return new Flat
            {
                Price = entity.Price,
                Rooms = entity.Rooms,
                Floor = entity.Floor,
                LivingArea = entity.LivingArea,
                Id = entity.Id,
            };
        }

        public static List<BaseRealEstate> ToDomain(this List<BaseRealEstateEntity> entities)
        {
            var domains = new List<BaseRealEstate>();
            foreach (var entity in entities)
            {
                if (entity is PrivateLandEntity privateLand)
                {
                    domains.Add(privateLand.ToDomain());
                } 
                else if (entity is FlatEntity flat)
                {
                    domains.Add(flat.ToDomain());
                }
            }
            return domains;
        }

        public static PrivateLandEntity ToEntity(this PrivateLand domain)
        {
            return new PrivateLandEntity
            {
                LandArea = domain.LandArea,
                Price = domain.Price,
                Id = domain.Id,
            };
        }

        public static FlatEntity ToEntity(this Flat domain)
        {
            return new FlatEntity
            {
                Rooms = domain.Rooms,
                Floor = domain.Floor,
                LivingArea = domain.LivingArea,
                Price = domain.Price,
                Id = domain.Id,
            };
        }

        public static List<BaseRealEstateEntity> ToEntity(this List<BaseRealEstate> domains)
        {
            var entities = new List<BaseRealEstateEntity>();
            foreach (var domain in domains)
            {
                if (domain is PrivateLand privateLand)
                {
                    entities.Add(privateLand.ToEntity());
                }
                else if (domain is Flat flat)
                {
                    entities.Add(flat.ToEntity());
                }
            }
            return entities;
        }
    }
}
