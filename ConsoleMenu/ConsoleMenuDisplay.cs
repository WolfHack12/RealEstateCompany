using ConsoleMenu.Abstract;
using Mappers;
using Models.Abstract;
using Services;
using Services.Abstract;
using System;
using System.Collections.Generic;

namespace ConsoleMenu
{
    public class ConsoleMenuDisplay : BaseConsoleMenu
    {
        private static string[] options =
        {
            "1. Display users;",
            "2. Display real estates;",
            "3. Back.",
        };

        private enum Option
        {
            DISPLAY_USERS = 1,
            DISPLAY_REAL_ESTATES,
            BACK,
        }

        private enum UsersSortOrder
        {
            DEFAULT = 1,
            FIRST_NAME,
            LAST_NAME,
            BANK_ACCOUNT,
            TYPE,
            PRICE,
            ERROR,
        }

        private enum RealEstatesSortOrder
        {
            DEFAULT = 1,
            TYPE,
            PRICE,
            ERROR,
        }

        private IRealEstateService realEstateService = new RealEstateService();
        private IUserService userService = new UserService();

        public ConsoleMenuDisplay()
            : base(options)
        { }

        protected sealed override ConsoleMode ProcessOption(int option)
        {
            Option action = (Option)option;
            switch (action)
            {
                case Option.DISPLAY_USERS:
                    DisplayOwners();
                    return ConsoleMode.CORRECT;
                case Option.DISPLAY_REAL_ESTATES:
                    DisplayRealEstates();
                    return ConsoleMode.CORRECT;
                case Option.BACK:
                    return ConsoleMode.QUIT;
                default:
                    Console.WriteLine(Message.DEFUNCT_OPTION);
                    return ConsoleMode.CORRECT;
            }
        }

        private void DisplayOwners()
        {
            var order = SetUserSortOrder();
            var users = SortOwners(order);
            if (users == null)
            {
                return;
            }
            if (users.Count < 1)
            {
                Console.WriteLine("No saved owners.");
                return;
            }
            string separator = new string('-', 17);
            Console.WriteLine(separator);
            foreach (var user in users)
            {
                Console.Write(user.ToString());
                Console.WriteLine(separator);
            }
        }

        private void DisplayRealEstates()
        {
            var order = SetRealEstateSortOrder();
            var realEstates = SortRealEstates(order);
            if (realEstates == null)
            {
                return;
            }
            if (realEstates.Count < 1)
            {
                Console.WriteLine("No saved real estates.");
                return;
            }
            string separator = new string('-', 17);
            Console.WriteLine(separator);
            foreach (var estate in realEstates)
            {
                Console.WriteLine(estate.ToString());
                Console.WriteLine(separator);
            }
        }

        private List<BaseUserModel> SortOwners(UsersSortOrder order)
        {
            var users = userService.GetList();
            switch (order)
            {
                case UsersSortOrder.DEFAULT:
                    return users.ToModel();
                case UsersSortOrder.FIRST_NAME:
                    return userService.SortOwnersByFirstName(users).ToModel();
                case UsersSortOrder.LAST_NAME:
                    return userService.SortOwnersByLastName(users).ToModel();
                case UsersSortOrder.BANK_ACCOUNT:
                    return userService.SortOwnersByBankAccount(users).ToModel();
                default:
                    Console.WriteLine("Invalid sort order.");
                    return null;
            }
        }

        private List<BaseRealEstateModel> SortRealEstates(RealEstatesSortOrder order)
        {
            var estates = realEstateService.GetList();
            switch (order)
            {
                case RealEstatesSortOrder.DEFAULT:
                    return estates.ToModel();
                case RealEstatesSortOrder.TYPE:
                    return realEstateService.SortByType(estates).ToModel();
                case RealEstatesSortOrder.PRICE:
                    return realEstateService.SortByPrice(estates).ToModel();
                default:
                    Console.WriteLine("Invalid sort order.");
                    return null;
            }
        }

        private UsersSortOrder SetUserSortOrder()
        {
            string message = "Sort order:\n1. Default;\n2. By first name;\n3. By second name;\n4. By bank account.\n";
            UsersSortOrder order = (UsersSortOrder)int.Parse(ReadDataInput(message, "[1-4]"));
            return order;
        }

        private RealEstatesSortOrder SetRealEstateSortOrder()
        {
            string message = "Sort order:\n1. Default;\n2. By type;\n3. By price.\n";
            RealEstatesSortOrder order = (RealEstatesSortOrder)int.Parse(ReadDataInput(message, "[1-3]"));
            return order;
        }
    }
}
