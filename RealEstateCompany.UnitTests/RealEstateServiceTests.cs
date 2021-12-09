using BLL;
using BLL.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateCompany.UnitTests
{
    [TestClass]
    public class RealEstateServiceTests
    {
        [TestMethod]
        public void SortByPrice_EmptyCollection_ReturnsEmptyCollection()
        {
            var realEstateService = new RealEstateService();
            var estates = new List<BaseRealEstate>();

            var sorted = realEstateService.SortByPrice(estates);

            Assert.AreEqual(0, sorted.Count);
        }

        [TestMethod]
        public void SortByPrice_GivenCollection_ParamsNotChanged()
        {
            var service = new RealEstateService();
            var estates = new List<BaseRealEstate>
            {
                new Flat { Price = 2 },
                new PrivateLand { Price = 1 },
            };
            var copy = new List<BaseRealEstate>(estates);

            service.SortByPrice(estates);

            Assert.AreEqual(copy[0].Price, estates[0].Price);
            Assert.AreEqual(copy[1].Price, estates[1].Price);
        }

        [TestMethod]
        public void SortByPrice_GivenCollection_ReturnsSortedCollection()
        {
            var service = new RealEstateService();
            var estates = new List<BaseRealEstate>
            {
                new Flat { Price = 2 },
                new PrivateLand { Price = 1 },
            };

            var sorted = service.SortByPrice(estates);

            Assert.AreEqual(sorted[0].Price, estates[1].Price);
            Assert.AreEqual(sorted[1].Price, estates[0].Price);
        }

        [TestMethod]
        public void SortByType_GivenCollection_ReturnsSortedCollection()
        {
            var service = new RealEstateService();
            var estates = new List<BaseRealEstate>
            {
                new PrivateLand { Price = 100 },
                new Flat { Rooms = 6, Price = 6 },
                new Flat { Rooms = 1, Price = 1 },
                new Flat { Rooms = 5, Price = 5 },
                new Flat { Rooms = 2, Price = 2 },
                new PrivateLand { Price = 200 },
                new PrivateLand { Price = 300 },
                new Flat { Rooms = 3, Price = 3 },
                new Flat { Rooms = 4, Price = 4 },
            };

            var sorted = service.SortByType(estates);

            Assert.AreEqual(sorted[6].Price, estates[0].Price);
            Assert.AreEqual(sorted[5].Price, estates[1].Price);
            Assert.AreEqual(sorted[0].Price, estates[2].Price);
            Assert.AreEqual(sorted[4].Price, estates[3].Price);
            Assert.AreEqual(sorted[1].Price, estates[4].Price);
            Assert.AreEqual(sorted[7].Price, estates[5].Price);
            Assert.AreEqual(sorted[8].Price, estates[6].Price);
            Assert.AreEqual(sorted[2].Price, estates[7].Price);
            Assert.AreEqual(sorted[3].Price, estates[8].Price);
        }

        [TestMethod]
        public void SortByType_EmptyCollection_ReturnsEmptyCollection()
        {
            var realEstateService = new RealEstateService();
            var estates = new List<BaseRealEstate>();

            var sorted = realEstateService.SortByType(estates);

            Assert.AreEqual(0, sorted.Count);
        }
    }
}
