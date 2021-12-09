using ConsoleMenu.Abstract;
using Exceptions;
using Mappers;
using Models;
using Services;
using Services.Abstract;
using System;

namespace ConsoleMenu
{
    public class ConsoleMenuAdd : BaseConsoleMenu
    {
        private static string[] options =
        {
            "1. Add owner;",
            "2. Add flat;",
            "3. Add private land;",
            "4. Back.",
        };

        private enum Option
        {
            ADD_OWNER = 1,
            ADD_FLAT,
            ADD_PRIVATE_LAND,
            BACK,
        }

        private struct Pattern
        {
            public const string NAME = @"[A-Z]{1}[a-z]+";
            public const string PHONE_NUMBER = @"\d+";
            public const string BANK_ACCOUNT = @"\d+";

            public const string ROOM = @"\d+";
            public const string FLOOR = @"\d+";
            public const string PRICE = @"\d+(.|,)?\d*";
            public const string AREA = @"\d+(.|,)?\d*";
        }

        private IRealEstateService realEstateService = new RealEstateService();
        private IUserService userService = new UserService();

        public ConsoleMenuAdd()
            : base(options)
        { }

        protected sealed override ConsoleMode ProcessOption(int option)
        {
            Option action = (Option)option;
            switch (action)
            {
                case Option.ADD_OWNER:
                    AddOwner();
                    return ConsoleMode.CORRECT;
                case Option.ADD_FLAT:
                    AddFlat();
                    return ConsoleMode.CORRECT;
                case Option.ADD_PRIVATE_LAND:
                    AddPrivateLand();
                    return ConsoleMode.CORRECT;
                case Option.BACK:
                    return ConsoleMode.QUIT;
                default:
                    Console.WriteLine(Message.DEFUNCT_OPTION);
                    return ConsoleMode.CORRECT;
            }
        }

        private void AddOwner()
        {
            string phone = ReadDataInput("Phone number: ", Pattern.PHONE_NUMBER);
            bool isExist = userService.IsExist(phone);
            if (isExist)
            {
                Console.WriteLine("User already exists.");
                return;
            }

            string firstName = ReadDataInput("First name: ", Pattern.NAME);
            string lastName = ReadDataInput("Last name: ", Pattern.NAME);
            string bankAccount = ReadDataInput("Bank account: ", Pattern.BANK_ACCOUNT);

            var owner = new OwnerUserModel(phone, firstName, lastName, bankAccount);
            userService.Add(owner.ToDomain());
        }

        private void AddPrivateLand()
        {
            string phone;

            try
            {
                phone = LoginUser();
            }
            catch (UnexistantUserException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            double landArea;
            double price;
            try
            {
                landArea = double.Parse(ReadDataInput("Land area: ", Pattern.AREA));
                price = double.Parse(ReadDataInput("Price: ", Pattern.PRICE));
            }
            catch (OverflowException)
            {
                Console.WriteLine(Message.BIG_VALUE);
                return;
            }            

            var privateLand = new PrivateLandModel(landArea, price);

            try
            {
                realEstateService.Add(privateLand.ToDomain(), phone);
            }
            catch (OverflowEstatesCollectionException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        private void AddFlat()
        {
            string phone;

            try
            {
                phone = LoginUser();
            }
            catch (UnexistantUserException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            int rooms;
            int floor;
            double livingArea;
            double price;
            try
            {
                rooms = int.Parse(ReadDataInput("Rooms: ", Pattern.ROOM));
                floor = int.Parse(ReadDataInput("Floor: ", Pattern.FLOOR));
                livingArea = double.Parse(ReadDataInput("Living area: ", Pattern.AREA));
                price = double.Parse(ReadDataInput("Price: ", Pattern.PRICE));
            }
            catch (OverflowException)
            {
                Console.WriteLine(Message.BIG_VALUE);
                return;
            }

            var flat = new FlatModel(rooms, floor, livingArea, price);

            try
            {
                realEstateService.Add(flat.ToDomain(), phone);
            }
            catch (OverflowEstatesCollectionException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        private string LoginUser()
        {
            string phone = ReadDataInput("Who adds estate (phone): ", Pattern.PHONE_NUMBER);
            bool isExist = userService.IsExist(phone);
            if (!isExist)
            {
                throw new UnexistantUserException($"User { phone } does not exist.");
            }
            return phone;
        }
    }
}
