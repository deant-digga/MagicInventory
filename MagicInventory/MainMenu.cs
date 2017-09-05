using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicInventory
{
    public class MainMenu : Menu
    {
       
        public static void RunMenu()
        {
            int menuLength = 6;
            string[] mainMenu =
            { " Welcome to marvellous Magic\n",
                " ==========================\n",
                "\t1. Owner\n", "\t2. Franchise Owner\n",
                "\t3. Customer\n",
                "\t4. Quit\n"};
            createMenu(menuLength, mainMenu);
            changeMenu();
        }

        public static void changeMenu()
        {
            Console.WriteLine(" Enter an option:");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    OwnerMenu.RunOwner();
                    break;
                case "2":
                    store = FranchiseMenu.SelectFranchisee();
                    FranchiseMenu.RunFranchise();
                    break;
                case "3":
                    CustomerMenu.RunCustomer();
                    break;
                case "4":
                    Environment.Exit(0);
                    return;
                default:
                    Console.WriteLine("Invaild Input");
                    changeMenu();
                    break;
            }
        }
    }
}
