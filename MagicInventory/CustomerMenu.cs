using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicInventory
{
    public class CustomerMenu : Menu
    {
        public static string storeNumber = null; 

        public static void RunCustomer()
        {
            selectStore();
            int menuLength = 6;
            string[] mainMenu = new string[]
            { " Welcome to marvellous Magic (Retail - " + storeNumber + ")\n",
                " ==========================\n",
                "\t1. Display Products\n",
                "\t2. Display Workshops\n",
                "\t4. Return to Main Menu\n",
                "\t5. Exit"};
            createMenu(menuLength, mainMenu);
            optionSelect();
        }

        static void selectStore()
        {
            string input;
            int menuLength = 7;
            string[] customerMenu = new string[]
            { " Welcome to marvellous Magic\n",
                " ==========================\n",
                "\t1. CBD\n",
                "\t2. North\n",
                "\t3. East\n",
                "\t4. South\n",
                "\t5. West\n"};
            createMenu(menuLength, customerMenu);
            Console.WriteLine("Please choose store location:");
            input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    storeNumber = "CBD";
                    break;
                case "2":
                    storeNumber = "North";
                    break;
                case "3":
                    storeNumber = "East";
                    break;
                case "4":
                    storeNumber = "South";
                    break;
                case "5":
                    storeNumber = "West";
                    break;
                default:
                    Console.WriteLine("Invaild location, Please try again.");
                    selectStore();
                    break;
            }
        }

        private static void optionSelect()
        {
            string input;
            Console.WriteLine("Enter an option:");
            input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    MainMenu.RunMenu();
                    break;
                case "5":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invaild Option");
                    optionSelect();
                    break;
            }
        }
    }
}
