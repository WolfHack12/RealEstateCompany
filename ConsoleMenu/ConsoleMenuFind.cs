using ConsoleMenu.Abstract;
using Mappers;
using Services;
using Services.Abstract;
using System;

namespace ConsoleMenu
{
    public class ConsoleMenuFind : BaseConsoleMenu
    {
        private static string[] options =
        {
            "1. Search by keyword in users;",
            "2. Search by keyword in real estates;",
            "3. Search by keyword everywhere;",
            "4. Extended user search;",
            "5. Back.",
        };

        private enum Option
        {
            BY_KEYWORD_IN_USERS = 1,
            BY_KEYWORD_IN_ESTATES,
            BY_KEYWORD_EVERYWHERE,
            EXTENDED_IN_USERS,
            BACK,
        }

        private IRealEstateService realEstateService = new RealEstateService();
        private IUserService userService = new UserService();

        public ConsoleMenuFind()
            : base(options)
        { }

        protected override ConsoleMode ProcessOption(int option)
        {
            Option action = (Option)option;
            switch (action)
            {
                case Option.BY_KEYWORD_IN_USERS:
                    {
                        string word = ReadDataInput("Keyword: ");
                        SearchByKeywordInUsers(word);
                        return ConsoleMode.CORRECT;
                    }
                case Option.BY_KEYWORD_IN_ESTATES:
                    {
                        string word = ReadDataInput("Keyword: ");
                        SearchByKeyWordInRealEstates(word);
                        return ConsoleMode.CORRECT;
                    }
                case Option.BY_KEYWORD_EVERYWHERE:
                    {
                        string word = ReadDataInput("Keyword: ");
                        Console.WriteLine("\tMatches in users:");
                        SearchByKeywordInUsers(word);
                        Console.WriteLine("\tMatches in real estates:");
                        SearchByKeyWordInRealEstates(word);
                        return ConsoleMode.CORRECT;
                    }
                case Option.EXTENDED_IN_USERS:
                    ExtendedOwnerSearch();
                    return ConsoleMode.CORRECT;
                case Option.BACK:
                    return ConsoleMode.QUIT;
                default:
                    Console.WriteLine(Message.DEFUNCT_OPTION);
                    return ConsoleMode.CORRECT;
            }
        }

        private void SearchByKeywordInUsers(string word)
        {
            var found = userService.FindUsersByKeyword(word).ToModel();
            if (found.Count < 1)
            {
                Console.WriteLine("No results.");
                return;
            }
            string separator = new string('-', 17);
            Console.WriteLine(separator);
            foreach (var user in found)
            {
                Console.Write(user.ToString());
                Console.WriteLine(separator);
            }
        }

        private void SearchByKeyWordInRealEstates(string word)
        {
            var found = realEstateService.SearchByKeyWord(word).ToModel();
            if (found.Count < 1)
            {
                Console.WriteLine("No results.");
                return;
            }
            string separator = new string('-', 17);
            Console.WriteLine(separator);
            foreach (var re in found)
            {
                Console.WriteLine(re.ToString());
                Console.WriteLine(separator);
            }
        }

        private void ExtendedOwnerSearch()
        {
            string firstName = ReadDataInput("First name: ", @"\w+");
            string lastName = ReadDataInput("Last name: ", @"\w+");
            string bankAccount = ReadDataInput("Bank account: ", @"\d+");
            var searched = userService.FindOwner(firstName, lastName, bankAccount)?.ToModel();
            if (searched == null)
            {
                Console.WriteLine("No results.");
                return;
            }
            Console.WriteLine("\tResult:");
            Console.WriteLine(searched.ToString());
        }
    }
}
