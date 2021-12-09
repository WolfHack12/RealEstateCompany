using ConsoleMenu.Abstract;
using System;

namespace ConsoleMenu
{
    public sealed class ConsoleMenuMain : BaseConsoleMenu
    {
        private static string[] options =
        {
            "1. Add;", 
            "2. Display;",
            "3. Edit;",
            "4. Delete;",
            "5. Find;", 
            "6. Quit.",
        };

        private enum Option
        {
            GOTO_ADD = 1,
            GOTO_DISPLAY,
            GOTO_EDIT,
            GOTO_DELETE,
            GOTO_FIND,
            QUIT,
        }

        private readonly ConsoleMenuAdd addMenu;
        private readonly ConsoleMenuDisplay displayMenu;
        private readonly ConsoleMenuEdit editMenu;
        private readonly ConsoleMenuDelete deleteMenu;
        private readonly ConsoleMenuFind findMenu;

        public ConsoleMenuMain()
            : base(options)
        {
            addMenu = new ConsoleMenuAdd();
            displayMenu = new ConsoleMenuDisplay();
            editMenu = new ConsoleMenuEdit();
            deleteMenu = new ConsoleMenuDelete();
            findMenu = new ConsoleMenuFind();
        }

        protected sealed override ConsoleMode ProcessOption(int option)
        {
            Option action = (Option)option;
            switch (action)
            {
                case Option.GOTO_ADD:
                    addMenu.Run();
                    return ConsoleMode.CORRECT;
                case Option.GOTO_DISPLAY:
                    displayMenu.Run();
                    return ConsoleMode.CORRECT;
                case Option.GOTO_EDIT:
                    editMenu.Run();
                    return ConsoleMode.CORRECT;
                case Option.GOTO_DELETE:
                    deleteMenu.Run();
                    return ConsoleMode.CORRECT;
                case Option.GOTO_FIND:
                    findMenu.Run();
                    return ConsoleMode.CORRECT;
                case Option.QUIT:
                    return ConsoleMode.QUIT;
                default:
                    Console.WriteLine(Message.DEFUNCT_OPTION);
                    return ConsoleMode.CORRECT;
            }
        }
    }
}
