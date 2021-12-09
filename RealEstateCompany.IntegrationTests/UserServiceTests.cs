using BLL;
using DAL;
using Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using Xunit.Sdk;

namespace RealEstateCompany.IntegrationTests
{
    [TestClass]
    public class UserServiceTests
    {
        [TestMethod]
        public void Add_OwnerWithoutEstates_WritesToFileWithNoDataLoss()
        {
            // Arrange
            new JSONDataProvider().Delete(@".\users.dat");
            var service = new UserService();
            var written = new OwnerUser
            {
                FirstName = "Alex",
                LastName = "Dobrin",
                BankAccount = "5375414124101113",
                Phone = "0990768327",
            };

            // Act
            service.Add(written);
            var read = service.GetList()[0] as OwnerUser;

            // Assert
            Assert.AreEqual(written.FirstName, read.FirstName);
            Assert.AreEqual(written.LastName, read.LastName);
            Assert.AreEqual(written.BankAccount, read.BankAccount);
            Assert.AreEqual(written.Phone, read.Phone);
        }

        [TestMethod]
        [ExpectedException(typeof(AddExistantUserException))]
        public void Add_SamePhoneUser_ThrowsAddExistantUserException()
        {
            // Arrange
            new JSONDataProvider().Delete(@".\users.dat");
            var service = new UserService();
            string samePhone = "0990768327";
            var writeFirst = new OwnerUser
            {
                Phone = samePhone,
            };
            var writeSecond = new OwnerUser
            {
                Phone = samePhone,
            };

            // Act
            service.Add(writeFirst);

            // Assert
            service.Add(writeSecond);
        }

        [TestMethod]
        public void Add_NextUser_IncreasesId()
        {
            // Arrange
            new JSONDataProvider().Delete(@".\users.dat");
            var service = new UserService();
            var user = new OwnerUser
            {
                Phone = "1",
            };
            var nextUser = new OwnerUser
            {
                Phone = "2",
            };

            // Act
            service.Add(user);
            service.Add(nextUser);
            var id = (service.GetList()[0] as OwnerUser).Id;
            var nextId = (service.GetList()[1] as OwnerUser).Id;

            // Assert
            Assert.AreEqual(id, nextId - 1);
        }

        [TestMethod]
        public void IsExist_UnexistantPhone_ReturnFalse()
        {
            // Arrange
            new JSONDataProvider().Delete(@".\users.dat");
            var service = new UserService();
            var user = new OwnerUser
            {
                Phone = "2",
            };

            // Act
            service.Add(user);

            // Assert
            Assert.IsFalse(service.IsExist("1"));
        }

        [TestMethod]
        public void IsExist_ExistantPhone_ReturnTrue()
        {
            // Arrange
            new JSONDataProvider().Delete(@".\users.dat");
            var service = new UserService();
            var user = new OwnerUser
            {
                Phone = "1",
            };

            // Act
            service.Add(user);

            // Assert
            Assert.IsTrue(service.IsExist("1"));
        }

        [TestMethod]
        public void CreateOwnerEstateById_EstateFound_DomainOwnerContainsDomainEstates()
        {
            // Arrange
            new JSONDataProvider().Delete(@".\users.dat");
            new JSONDataProvider().Delete(@".\realEstates.dat");
            var userService = new UserService();
            var user = new OwnerUser
            {
                Phone = "1",
            };
            var realEstateService = new RealEstateService();
            var flat = new Flat
            {
                Price = 100,
            };
            var privateLand = new PrivateLand
            {
                Price = 500,
            };

            // Act
            userService.Add(user);
            realEstateService.Add(flat, user.Phone);
            realEstateService.Add(privateLand, user.Phone);

            var ownerRealEstates = (userService.GetList()[0] as OwnerUser).RealEstatesList;

            // Assert
            Assert.AreEqual(ownerRealEstates[0].Price, flat.Price);
            Assert.AreEqual(ownerRealEstates[1].Price, privateLand.Price);
        }

        [TestMethod]
        public void CreateOwnerEstateById_EstateNotFound_DomainOwnerContainsEmptyCollection()
        {
            // Arrange
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var user = new OwnerUser
            {
                Phone = "1",
            };

            // Act
            userService.Add(user);

            var ownerRealEstates = (userService.GetList()[0] as OwnerUser).RealEstatesList;

            // Assert
            Assert.AreEqual(0, ownerRealEstates.Capacity);
        }

        [TestMethod]
        [ExpectedException(typeof(UnexistantUserException))]
        public void DeleteOwner_UnexistentOwner_ThrowsUnexistantUserException()
        {
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var user = new OwnerUser
            {
                Phone = "1",
            };

            userService.DeleteOwner(user.Phone);
        }

        [TestMethod]
        public void DeleteOwner_ExistatntOwner_DeletesOwnerFromFile()
        {
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var user = new OwnerUser
            {
                Phone = "1",
            };

            userService.Add(user);
            userService.DeleteOwner(user.Phone);

            Assert.AreEqual(0, userService.GetList().Count);
        }

        [TestMethod]
        public void ChangeOwner_ExistantOwner_ChangesDataInFile()
        {
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var user = new OwnerUser
            {
                Phone = "1",
                FirstName = "Alex",
            };
            var toChange = new OwnerUser
            {
                Phone = "2",
                FirstName = "Johan",
            };

            userService.Add(user);
            userService.ChangeOwner(toChange, user.Phone);

            Assert.AreNotEqual(user.FirstName, (userService.GetList()[0] as OwnerUser).FirstName);
        }

        [TestMethod]
        [ExpectedException(typeof(UnexistantUserException))]
        public void ChangeOwner_UnexistantOwner_ChangesDataInFile()
        {
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var toChange = new OwnerUser
            {
                Phone = "2",
                FirstName = "Johan",
            };

            userService.ChangeOwner(toChange, "2");
        }

        [TestMethod]
        public void FindByKeyword_EmptyUsersCollection_ReturnsEmptyCollection()
        {
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();

            var found = userService.FindUsersByKeyword("Alex");

            Assert.AreEqual(0, found.Count);
        }

        [TestMethod]
        public void FindByKeyword_UsersCollection_ReturnsUsersWithMathces()
        {
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var user = new OwnerUser
            {
                Phone = "1",
                FirstName = "Alex",
            };
            var user2 = new OwnerUser
            {
                Phone = "2",
                FirstName = "Johan",
            };
            var user3 = new OwnerUser
            {
                Phone = "3",
                FirstName = "Alex",
            };

            userService.Add(user);
            userService.Add(user2);
            userService.Add(user3);

            var found = userService.FindUsersByKeyword("Alex");

            Assert.AreEqual("Alex", (found[0] as OwnerUser).FirstName);
            Assert.AreEqual("Alex", (found[1] as OwnerUser).FirstName);
            Assert.AreEqual(2, found.Count);
        }

        [TestMethod]
        public void FindByKeyword_noMatchingUser_ReturnsEmptyCollection()
        {
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var user = new OwnerUser
            {
                Phone = "1",
                FirstName = "Alex",
            };
            var user2 = new OwnerUser
            {
                Phone = "2",
                FirstName = "Johan",
            };
            var user3 = new OwnerUser
            {
                Phone = "3",
                FirstName = "Alex",
            };

            userService.Add(user);
            userService.Add(user2);
            userService.Add(user3);

            var found = userService.FindUsersByKeyword("4");

            Assert.AreEqual(0, found.Count);
        }

        [TestMethod]
        public void FindByKeyword_FlatInUserEstatesCollection_ReturnsUsersWithMathces()
        {
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var user = new OwnerUser
            {
                Phone = "1",
                FirstName = "Alex",
            };
            userService.Add(user);

            var flat = new Flat { Floor = 40 };
            new RealEstateService().Add(flat, user.Phone);

            var found = userService.FindUsersByKeyword("40");

            Assert.AreEqual("Alex", (found[0] as OwnerUser).FirstName);
            Assert.AreEqual(1, found.Count);
        }

        [TestMethod]
        public void FindByKeyword_PrivateLandInUserEstatesCollection_ReturnsUsersWithMathces()
        {
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var user = new OwnerUser
            {
                Phone = "1",
                FirstName = "Alex",
            };
            userService.Add(user);

            var pl = new PrivateLand { LandArea = 40 };
            new RealEstateService().Add(pl, user.Phone);

            var found = userService.FindUsersByKeyword("40");

            Assert.AreEqual("Alex", (found[0] as OwnerUser).FirstName);
            Assert.AreEqual(1, found.Count);
        }

        [TestMethod]
        public void FindByKeyword_UnexistantPrivateLandInUserEstatesCollection_ReturnsEmptyCollection()
        {
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var user = new OwnerUser
            {
                Phone = "1",
                FirstName = "Alex",
            };
            userService.Add(user);

            var found = userService.FindUsersByKeyword("40");

            Assert.AreEqual(0, found.Count);
        }

        [TestMethod]
        public void FindOwner_ExistantOwner_ReturnsSearchedOwner()
        {
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var user = new OwnerUser
            {
                Phone = "1",
                FirstName = "Alex",
                LastName = "Dobrin",
                BankAccount = "123123",
            };
            userService.Add(user);
            var pl = new PrivateLand { LandArea = 40 };
            new RealEstateService().Add(pl, user.Phone);
            var found = userService.FindOwner(user.FirstName, user.LastName, user.BankAccount);

            Assert.AreEqual(found.Phone, user.Phone);
        }

        [TestMethod]
        public void FindOwner_UnexistantOwner_ReturnsNull()
        {
            new JSONDataProvider().Delete(@".\users.dat");
            var userService = new UserService();
            var found = userService.FindOwner("Alex", "Alex", "Alex");

            Assert.AreEqual(null, found);
        }
    }
}
