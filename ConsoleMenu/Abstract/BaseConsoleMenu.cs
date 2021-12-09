using Exceptions;
using PL.Abstract;
using System;
using System.Text.RegularExpressions;

namespace ConsoleMenu.Abstract
{
    public abstract class BaseConsoleMenu : IMenu<int>, IRunableMenu
    {
        private readonly string[] options;

        protected struct Message
        {
            public const string ERR_MESSAGE = "Incorrect use of the application.";
            public const string DEFUNCT_OPTION = "Option does not exist.";
            public const string UNDEFINED_OPTIONS = "Empty options list.";
            public const string BIG_VALUE = "We can not process such a big values at the moment.";
        }

        protected enum ConsoleMode
        {
            ERROR = -1,
            QUIT = 0,
            CORRECT = 1,
        }

        public BaseConsoleMenu(string[] options = null)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            this.options = options ?? new string[] { Message.UNDEFINED_OPTIONS };
        }

        public virtual void Run()
        {
            while (true)
            {
                RenderOptions();
                ConsoleMode mode = SetMode();

                if (mode == ConsoleMode.ERROR || 
                    mode == ConsoleMode.QUIT)
                {
                    break;
                }
            }
        }

        public virtual void RenderOptions()
        {
            foreach (var option in options)
            {
                Console.WriteLine(option);
            }
        }

        private ConsoleMode SetMode()
        {
            try
            {
                int option = ReadOption();
                return ProcessOption(option);
            }
            catch (InvalidOptionException ex)
            {
                Console.WriteLine(ex.Message);
                return ConsoleMode.ERROR;
            }
        }

        public virtual int ReadOption()
        {
            string input = Console.ReadLine();
            bool isValid = ValidateInput(input, @"[1-9]{1,2}");
            if (!isValid)
            {
                throw new InvalidOptionException(Message.ERR_MESSAGE);
            }
            return int.Parse(input);
        }

        protected virtual bool ValidateInput(string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }

        protected abstract ConsoleMode ProcessOption(int option);

        protected virtual string ReadDataInput(string message, string pattern = "")
        {   
            if (pattern != "")
            {
                while (true)
                {
                    Console.Write(message);
                    string input = Console.ReadLine();

                    bool isValidInput = ValidateInput(input, pattern);
                    if (isValidInput)
                    {
                        return input.Trim();
                    }
                }
            }    
            else
            {
                Console.Write(message);
                return Console.ReadLine().Trim();
            }
        }
    }
}
