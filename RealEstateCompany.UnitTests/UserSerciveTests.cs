using BLL;
using BLL.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using System.Collections.Generic;

namespace RealEstateCompany.UnitTests
{
    [TestClass]
    public class UserSerciveTests
    {
        [TestMethod]
        public void SortOwnersByFirstName_EmptyCollection_ReturnsEmptyCollection()
        {
            var service = new UserService();
            var owners = new List<BaseUser>();

            var sorted = service.SortOwnersByFirstName(owners);

            Assert.AreEqual(0, sorted.Count);
        }

        [TestMethod]
        public void SortOwnersByFirstName_GivenCollection_ParamsNotChanged()
        {
            var service = new UserService();
            var owners = new List<BaseUser>
            {
                new OwnerUser { FirstName = "Johan" },
                new OwnerUser { FirstName = "Alex" },
            };
            var copy = new List<BaseUser>(owners);

            service.SortOwnersByFirstName(owners);

            Assert.AreEqual((copy[0] as OwnerUser).FirstName, (owners[0] as OwnerUser).FirstName);
            Assert.AreEqual((copy[1] as OwnerUser).FirstName, (owners[1] as OwnerUser).FirstName);
        }

        [TestMethod]
        public void SortOwnersByFirstName_DoesNotChangesParams_ReturnsSortedCollection()
        {
            var service = new UserService();
            var owners = new List<BaseUser>
            {
                new OwnerUser { FirstName = "Johan" },
                new OwnerUser { FirstName = "Alex" },
            };

            var sorted = service.SortOwnersByFirstName(owners);

            Assert.AreEqual((sorted[0] as OwnerUser).FirstName, (owners[1] as OwnerUser).FirstName);
            Assert.AreEqual((sorted[1] as OwnerUser).FirstName, (owners[0] as OwnerUser).FirstName);
        }

        [TestMethod]
        public void SortOwnersByLastName_EmptyCollection_ReturnsEmptyCollection()
        {
            var service = new UserService();
            var owners = new List<BaseUser>();

            var sorted = service.SortOwnersByLastName(owners);

            Assert.AreEqual(0, sorted.Count);
        }

        [TestMethod]
        public void SortOwnersByLastName_GivenCollection_ParamsNotChanged()
        {
            var service = new UserService();
            var owners = new List<BaseUser>
            {
                new OwnerUser { LastName = "Johan" },
                new OwnerUser { LastName = "Alex" },
            };
            var copy = new List<BaseUser>(owners);

            service.SortOwnersByLastName(owners);

            Assert.AreEqual((copy[0] as OwnerUser).LastName, (owners[0] as OwnerUser).LastName);
            Assert.AreEqual((copy[1] as OwnerUser).LastName, (owners[1] as OwnerUser).LastName);
        }

        [TestMethod]
        public void SortOwnersByLastName_DoesNotChangesParams_ReturnsSortedCollection()
        {
            var service = new UserService();
            var owners = new List<BaseUser>
            {
                new OwnerUser { LastName = "Johan" },
                new OwnerUser { LastName = "Alex" },
            };

            var sorted = service.SortOwnersByLastName(owners);

            Assert.AreEqual((sorted[0] as OwnerUser).LastName, (owners[1] as OwnerUser).LastName);
            Assert.AreEqual((sorted[1] as OwnerUser).LastName, (owners[0] as OwnerUser).LastName);
        }

        [TestMethod]
        public void SortOwnersByBankAccount_EmptyCollection_ReturnsEmptyCollection()
        {
            var service = new UserService();
            var owners = new List<BaseUser>();

            var sorted = service.SortOwnersByBankAccount(owners);

            Assert.AreEqual(0, sorted.Count);
        }

        [TestMethod]
        public void SortOwnersByBankAccount_GivenCollection_ParamsNotChanged()
        {
            var service = new UserService();
            var owners = new List<BaseUser>
            {
                new OwnerUser { BankAccount = "2" },
                new OwnerUser { BankAccount = "1" },
            };
            var copy = new List<BaseUser>(owners);

            service.SortOwnersByBankAccount(owners);

            Assert.AreEqual((copy[0] as OwnerUser).BankAccount, (owners[0] as OwnerUser).BankAccount);
            Assert.AreEqual((copy[1] as OwnerUser).BankAccount, (owners[1] as OwnerUser).BankAccount);
        }

        [TestMethod]
        public void SortOwnersByBankAccount_GivenCollection_ReturnsSortedCollection()
        {
            var service = new UserService();
            var owners = new List<BaseUser>
            {
                new OwnerUser { BankAccount = "2" },
                new OwnerUser { BankAccount = "1" },
            };

            var sorted = service.SortOwnersByBankAccount(owners);

            Assert.AreEqual((sorted[0] as OwnerUser).BankAccount, (owners[1] as OwnerUser).BankAccount);
            Assert.AreEqual((sorted[1] as OwnerUser).BankAccount, (owners[0] as OwnerUser).BankAccount);
        }
    }
}
