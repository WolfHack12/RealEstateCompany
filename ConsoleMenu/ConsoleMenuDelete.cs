using ConsoleMenu.Abstract;
using Exceptions;
using Mappers;
using Services;
using Services.Abstract;
using System;

namespace ConsoleMenu
{
    public class ConsoleMenuDelete : BaseConsoleMenu
    {
        private static string[] options =
        {
            "1. Delete owner.",
            "2. Delete real estate;",
            "3. Back."
        };

        private enum Option
        {
            DELETE_OWNER = 1,
            DELETE_REAL_ESTATE,
            BACK,
        }

        private IRealEstateService realEstateService = new RealEstateService();
        private IUserService userService = new UserService();

        public ConsoleMenuDelete()
            : base(options)
        { }

        protected sealed override ConsoleMode ProcessOption(int option)
        {
            Option action = (Option)option;
            switch (action)
            {
                case Option.DELETE_OWNER:
                    DeleteOwner();
                    return ConsoleMode.CORRECT;
                case Option.DELETE_REAL_ESTATE:
                    DeleteRealEstate();
                    return ConsoleMode.CORRECT;
                case Option.BACK:
                    return ConsoleMode.QUIT;
                default:
                    Console.WriteLine(Message.DEFUNCT_OPTION);
                    return ConsoleMode.CORRECT;
            }
        }

        private void DeleteOwner()
        {
            string phone;
            try
            {
                phone = LoginUser("Phone of user to delete: ");
            }
            catch (UnexistantUserException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            userService.DeleteOwner(phone);
        }

        private void DeleteRealEstate()
        {
            string phone;
            try
            {
                phone = LoginUser("Who deletes estate (phone): ");
            }
            catch (UnexistantUserException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            var realEstates = realEstateService.GetOwnerEstates(phone).ToModel();
            if (realEstates.Count < 1)
            {
                Console.WriteLine("User does not have estates.");
                return;
            }
            int i = 1;
            foreach (var estate in realEstates)
            {
                Console.WriteLine("\t" + i);
                Console.WriteLine(estate.ToString());
                i++;
            }
            int deleteIndex = int.Parse(ReadDataInput("Delete estate at position: ", @"[1-9]+"));
            try
            {
                realEstateService.Delete(phone, deleteIndex - 1);
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Invalid position.");
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
