using ConsoleMenu.Abstract;
using Exceptions;
using Mappers;
using Models;
using Services;
using Services.Abstract;
using System;

namespace ConsoleMenu
{
    public class ConsoleMenuEdit : BaseConsoleMenu
    {
        private static string[] options =
           {
            "1. Edit owner;",
            "2. Edit flat;",
            "3. Edit private land;",
            "4. Back.",
        };

        private enum Option
        {
            EDIT_OWNER = 1,
            EDIT_FLAT,
            EDIT_PRIVATE_LAND,
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

        public ConsoleMenuEdit()
            : base(options)
        { }

        protected override ConsoleMode ProcessOption(int option)
        {
            Option action = (Option)option;
            switch (action)
            {
                case Option.EDIT_OWNER:
                    EditOwner();
                    return ConsoleMode.CORRECT;
                case Option.EDIT_FLAT:
                    EditFlat();
                    return ConsoleMode.CORRECT;
                case Option.EDIT_PRIVATE_LAND:
                    EditPrivateLand();
                    return ConsoleMode.CORRECT;
                case Option.BACK:
                    return ConsoleMode.QUIT;
                default:
                    Console.WriteLine(Message.DEFUNCT_OPTION);
                    return ConsoleMode.CORRECT;
            }
        }

        private void EditOwner()
        {
            string oldPhone;
            try
            {
                oldPhone = LoginUser("Owners old phone: ");
            }
            catch (UnexistantUserException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            var editData = new OwnerUserModel();
            editData.Phone = ReadDataInput("New phone number: ", Pattern.PHONE_NUMBER);
            editData.FirstName = ReadDataInput("First name: ", Pattern.NAME);
            editData.LastName = ReadDataInput("Last name: ", Pattern.NAME);
            editData.BankAccount = ReadDataInput("Bank account: ", Pattern.BANK_ACCOUNT);

            userService.ChangeOwner(editData.ToDomain(), oldPhone);
        }

        private void EditFlat()
        {
            string phone;
            try
            {
                phone = LoginUser("Flat owner phone: ");
            }
            catch (UnexistantUserException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            var flats = realEstateService.GetOwnerEstates(phone).ToModel();
            if (flats.Count < 1)
            {
                Console.WriteLine("User does not have real estates.");
                return;
            }
            int i = 1;
            foreach (var flat in flats)
            {
                Console.WriteLine("\t" + i);
                Console.WriteLine(flat.ToString());
                i++;
            }
            int editIndex = int.Parse(ReadDataInput("Edit flat at position: ", @"[1-9]+"));

            var editData = new FlatModel();
            editData.Price = double.Parse(ReadDataInput("Price: ", Pattern.PRICE));
            editData.Rooms = int.Parse(ReadDataInput("Rooms: ", Pattern.ROOM));
            editData.Floor = int.Parse(ReadDataInput("Floor: ", Pattern.FLOOR));
            editData.LivingArea = double.Parse(ReadDataInput("Living area: ", Pattern.AREA));

            try
            {
                realEstateService.EditFlat(phone, editIndex - 1, editData.ToDomain());
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Invalid position.");
            }
            catch (UnexpectedRealEstateTypeException)
            {
                Console.WriteLine($"Real estate at position { editIndex } was not flat. Changes cancelled.");
            }
        }

        private void EditPrivateLand()
        {
            string phone;
            try
            {
                phone = LoginUser("Private land owner phone: ");
            }
            catch (UnexistantUserException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            var privateLands = realEstateService.GetOwnerEstates(phone).ToModel();
            if (privateLands.Count < 1)
            {
                Console.WriteLine("User does not have real estates.");
                return;
            }
            int i = 1;
            foreach (var privateLand in privateLands)
            {
                Console.WriteLine("\t" + i);
                Console.WriteLine(privateLand.ToString());
                i++;
            }
            int editIndex = int.Parse(ReadDataInput("Private land flat at position: ", @"[1-9]+"));

            var editData = new PrivateLandModel();
            editData.Price = double.Parse(ReadDataInput("Price: ", Pattern.PRICE));
            editData.LandArea = double.Parse(ReadDataInput("Land area: ", Pattern.AREA));

            try
            {
                realEstateService.EditPrivateLand(phone, editIndex - 1, editData.ToDomain());
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Invalid position.");
            }
            catch (UnexpectedRealEstateTypeException)
            {
                Console.WriteLine($"Real estate at position { editIndex } was not private land. Changes cancelled.");
            }
        }

        private string LoginUser(string msg)
        {
            string phone = ReadDataInput(msg, @"\d+");
            bool isExist = userService.IsExist(phone);
            if (!isExist)
            {
                throw new UnexistantUserException($"User { phone } does not exist.");
            }
            return phone;
        }
    }
}
