using DAL;
using DAL.Abstract;
using Entities;
using Entities.Abstract;
using Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Repositories
{
    public class RealEstateRepository : IRealEstateRepository
    {
        public string Path { get; set; }
        public Type[] SubclassTypes { get; set; }
        public IDataProvider DataProvider { get; set; }
        
        private static List<BaseRealEstateEntity> collection;

        public RealEstateRepository()
        {
            Path = @".\realEstates.dat";
            SubclassTypes = new Type[]
            {
                typeof(BaseRealEstateEntity),
                typeof(PrivateLandEntity),
                typeof(FlatEntity),
            };
            DataProvider = new JSONDataProvider();
            collection = Refresh();
        }

        public List<BaseRealEstateEntity> GetList()
        {
            collection = Refresh();
            return collection;
        }

        public void Add(BaseRealEstateEntity realEstate)
        {
            collection.Add(realEstate);
            Save();
        }

        public void Update(List<BaseRealEstateEntity> estates)
        {
            DataProvider.Delete(Path);
            collection = estates;
            Save();
        }

        public BaseRealEstateEntity FindById(int id)
        {
            collection = Refresh();
            return collection.Find(re => re.Id == id);
        }

        private void Save()
        {
            DataProvider.Write(Path, collection, typeof(List<BaseRealEstateEntity>), SubclassTypes);
        }

        private List<BaseRealEstateEntity> Refresh()
        {
            List<BaseRealEstateEntity> realEstates;

            try
            {
                realEstates = DataProvider
                    .Read(Path, typeof(List<BaseRealEstateEntity>), SubclassTypes)
                    as List<BaseRealEstateEntity>;
            }
            catch (SerializationException)
            {
                realEstates = null;
            }
            catch (InvalidOperationException)
            {
                realEstates = null;
            }

            return realEstates ?? new List<BaseRealEstateEntity>();
        }
    }
}
