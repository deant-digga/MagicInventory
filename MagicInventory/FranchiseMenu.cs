using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MagicInventory
{
    public class FranchiseMenu : Menu
    {
        public static string location;

        public static void RunFranchise()
        {
            int menuLength = 7;
            string[] mainMenu = new string[]
            { " Welcome to marvellous Magic (Franchise Holder - " + location + ")\n",
                " ==========================\n",
                "\t1. Display Inventory\n",
                "\t2. Display Inventory (Threshold)\n",
                "\t3. Add New Inventory Item\n",
                "\t4. Return to Main Menu\n",
                "\t5. Exit"};
            createMenu(menuLength, mainMenu);
            SelectOption();
        }

        public static List<Franchisee> SelectFranchisee()
        {
            Console.WriteLine("Enter store ID (north, east, south or west):");
            location = Console.ReadLine();
            switch (location)
            {
                case "north":
                    return JsonConvert.DeserializeObject<List<Franchisee>>(File.ReadAllText("north_inventory.json"));
                case "south":
                    return JsonConvert.DeserializeObject<List<Franchisee>>(File.ReadAllText("south_inventory.json"));
                case "west":
                    return JsonConvert.DeserializeObject<List<Franchisee>>(File.ReadAllText("west_inventory.json"));
                case "east":
                    return JsonConvert.DeserializeObject<List<Franchisee>>(File.ReadAllText("east_inventory.json"));
                default:
                    Console.WriteLine("Invaild ID please try again");
                    store = SelectFranchisee();
                    RunFranchise();
                    return null;
            }
        }

        static void SelectOption()
        {
            string input;
            Console.WriteLine("Enter an option:");
            input = Console.ReadLine();
            switch (input)
            {
                case ("1"):
                    DisplayStock();
                    break;
                case ("2"):
                    break;
                case ("3"):
                    DisplayStock();
                    break;
                case ("4"):
                    MainMenu.RunMenu();
                    break;
                case ("5"):
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invaild option");
                    RunFranchise();
                    break;
            }
        }

        static void DisplayStock()
        {
            string input1;
            string input2;
            Console.Write("Enter threshold (Press Q to go back):");
            input1 = Console.ReadLine();
            
            // check to see if user input is in range of requests index
            int threshold = 0;
            bool isNumeric = int.TryParse(input1, out threshold);
            while (isNumeric == false)
            {
                if (input1 == "q")
                {
                    RunFranchise();
                }
                Console.WriteLine("Invaild number");
                Console.Write("Enter threshold (Press Q to go back):");
                input1 = Console.ReadLine();
                isNumeric = int.TryParse(input1, out threshold);
            }

            // display menu
            Console.Clear();
            Console.WriteLine("Inventory\n");
            Console.Write("ID".PadRight(5));
            Console.Write("Product".PadRight(25));
            Console.Write("Current Stock".PadRight(15));
            Console.WriteLine("Re-Stock");
            Console.WriteLine("-------------------------------------");
            foreach (Franchisee store1 in store)
            {
                if (store1.StockLevel <= Convert.ToInt32(input1))
                {
                    store1.restock = true;
                }else{
                    store1.restock = false;
                }
                Console.WriteLine("{0}{1}{2}{3}", store1.Id.ToString().PadRight(5), store1.Name.PadRight(25), store1.StockLevel.ToString().PadRight(15), store1.restock);
            }
            Console.Write("\nEnter Request to process (Press Q to go back):");
            input2 = Console.ReadLine();
            int requestProcess = 0;
            isNumeric = int.TryParse(input2, out requestProcess);
            if (requestProcess <= store.Count && isNumeric == true)
            {
                if (store[requestProcess].restock == true)
                {
                    AddStockRequest(input1, input2);
                }else
                {
                    Console.WriteLine("The stock requested is above the threshold, do you wish to continue?(yes or no)");
                    string answer = Console.ReadLine();
                    if (answer == "yes")
                    {
                        AddStockRequest(input1, input2);
                    }else if (answer == "no")
                    {
                        DisplayStock();
                    }else
                    {
                        Console.WriteLine("Invaild Option");
                        DisplayStock();
                    }
                }
            }else if (isNumeric == false && input2 == "q")
            {
                RunFranchise();
            }
            else if (requestProcess > store.Count || isNumeric == false)
            {
                Console.WriteLine("Invaild Input");
                RunFranchise();
            }
            
        }

        static void AddStockRequest(string input1, string input2)
        {
            List<StockRequests> stockRequest = LoadStockRequest();
            Franchisee new1 = store[Convert.ToInt32(input2) - 1];
            stockRequest.Add(new StockRequests() { Id = (stockRequest.Count) + 1, Store = location, Name = new1.Name, Quantity = Convert.ToInt32(input1)});
            File.WriteAllText("stockrequests.json", JsonConvert.SerializeObject(stockRequest, Formatting.Indented));
            RunFranchise();
        }
    }
}
