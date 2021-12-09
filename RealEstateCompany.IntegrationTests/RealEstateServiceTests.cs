using BLL;
using DAL;
using Entities;
using Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repositories;
using Services;

namespace RealEstateCompany.IntegrationTests
{
    [TestClass]
    public class RealEstateServiceTests
    {
        [TestMethod]
        public void Add_RealEstate_RealEstateIdEntersRightOwnerCollection()
        {
            // Arrange
            new JSONDataProvider().Delete(@".\realEstates.dat");
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var userRepository = new UserRepository();
            var owner = new OwnerUser
            {
                Phone = "1",
            };
            var realEstateService = new RealEstateService();
            var written = new PrivateLand();

            // Act
            userService.Add(owner);

            realEstateService.Add(written, owner.Phone);
            var createdEstateId = (realEstateService.GetList()[0] as PrivateLand).Id;

            var readedOwner = userRepository.FindByPhone(owner.Phone) as OwnerUserEntity;
            var enteredToUserCollectionEstateId = readedOwner.RealEstatesList[0];

            // Assert
            Assert.AreEqual(createdEstateId, enteredToUserCollectionEstateId);
        }

        [TestMethod]
        public void Add_PrivateLand_WritesToFileWithNoDataLoss()
        {
            // Arrange
            new JSONDataProvider().Delete(@".\realEstates.dat");
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var owner = new OwnerUser
            {
                Phone = "1",
            };
            var realEstateService = new RealEstateService();
            var written = new PrivateLand
            {
                LandArea = 100,
                Price = 1000,
            };

            // Act
            userService.Add(owner);
            realEstateService.Add(written, owner.Phone);
            var read = realEstateService.GetList()[0] as PrivateLand;

            // Assert
            Assert.AreEqual(written.LandArea, read.LandArea);
            Assert.AreEqual(written.Price, read.Price);
        }

        [TestMethod]
        public void Add_Flat_WritesToFileWithNoDataLoss()
        {
            // Arrange
            new JSONDataProvider().Delete(@".\realEstates.dat");
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var owner = new OwnerUser
            {
                Phone = "1",
            };
            var realEstateService = new RealEstateService();
            var written = new Flat
            {
                Floor = 1,
                Rooms = 1,
                LivingArea = 1,
                Price = 1,
            };

            // Act
            userService.Add(owner);
            realEstateService.Add(written, owner.Phone);
            var read = realEstateService.GetList()[0] as Flat;

            // Assert
            Assert.AreEqual(written.Floor, read.Floor);
            Assert.AreEqual(written.LivingArea, read.LivingArea);
            Assert.AreEqual(written.Rooms, read.Rooms);
            Assert.AreEqual(written.Price, read.Price);
        }

        [TestMethod]
        [ExpectedException(typeof(UnexistantUserException))]
        public void Add_RealEstateToUnexistantUser_ThrowsAddUnexistantUserException()
        {
            // Arrange
            new JSONDataProvider().Delete(@".\realEstates.dat");
            var realEstateService = new RealEstateService();
            var written = new Flat
            {
                Floor = 1,
                Rooms = 1,
                LivingArea = 1,
                Price = 1,
            };
            string unexistantPhone = "404";

            // Act
            realEstateService.Add(written, unexistantPhone);

            // Assert
        }

        [TestMethod]
        public void Add_NextRealEstate_IncreasesId()
        {
            // Arrange
            new JSONDataProvider().Delete(@".\realEstates.dat");
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var owner = new OwnerUser
            {
                Phone = "1",
            };

            var realEstatesService = new RealEstateService();
            var estate = new Flat();
            var nextEstate = new PrivateLand();

            // Act
            userService.Add(owner);

            realEstatesService.Add(estate, owner.Phone);
            realEstatesService.Add(nextEstate, owner.Phone);
            var id = (realEstatesService.GetList()[0] as Flat).Id;
            var nextId = (realEstatesService.GetList()[1] as PrivateLand).Id;

            // Assert
            Assert.AreEqual(id, nextId - 1);
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowEstatesCollectionException))]
        public void Add_RealEstateToOwnerFilledEstatesCollection_ThrowsOverflowEstatesCollectionException()
        {
            // Arrange
            new JSONDataProvider().Delete(@".\realEstates.dat");
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var owner = new OwnerUser
            {
                Phone = "1",
            };

            var realEstatesService = new RealEstateService();
            var estate = new Flat();

            // Act
            userService.Add(owner);

            realEstatesService.Add(estate, owner.Phone);
            for (int i = 0; i <= OwnerUser.MAX_ESTAES - 1; i++)
            {
                realEstatesService.Add(estate, owner.Phone);
            }

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(System.IndexOutOfRangeException))]
        public void EditFlat_WrongIndex_ThrowsIndexOutOfRangeException()
        {
            // Arrange
            new JSONDataProvider().Delete(@".\realEstates.dat");
            var owner = new OwnerUser
            {
                Phone = "1",
            };

            var realEstatesService = new RealEstateService();
            var estate = new Flat();

            // Act
            realEstatesService.EditFlat(owner.Phone, 33, estate);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedRealEstateTypeException))]
        public void EditFlat_ChosenPrivateLandIndex_ThrowsUnexpectedRealEstateTypeException()
        {
            new JSONDataProvider().Delete(@".\realEstates.dat");
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var owner = new OwnerUser
            {
                Phone = "1",
            };

            var realEstatesService = new RealEstateService();
            var flat = new Flat();
            var pl = new PrivateLand();

            userService.Add(owner);
            realEstatesService.Add(flat, owner.Phone);
            realEstatesService.Add(pl, owner.Phone);

            realEstatesService.EditFlat(owner.Phone, 1, flat);
        }

        [TestMethod]
        public void EditFlat_GivenNewFlatData_ChangesDataInFile()
        {
            // Arrange
            new JSONDataProvider().Delete(@".\realEstates.dat");
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var owner = new OwnerUser
            {
                Phone = "1",
            };

            var realEstatesService = new RealEstateService();
            var flat = new Flat();
            var newFlat = new Flat { Price = 10000 };

            // Act
            userService.Add(owner);
            realEstatesService.Add(flat, owner.Phone);
            realEstatesService.EditFlat(owner.Phone, 0, newFlat);

            var list = realEstatesService.GetList();

            //Assert
            Assert.AreNotEqual(flat.Price, list[0].Price);
        }
    }
}
