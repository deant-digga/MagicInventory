using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MagicInventory
{
    class OwnerMenu : Menu
    {
        public static List<Owner> owner = LoadOwnerStock();

        public static void RunOwner()
        {
            int menuLength = 7;
            string[] mainMenu = new string[] 
            { " Welcome to marvellous Magic (Owner)\n",
                " ==========================\n",
                "\t1. Display All Stock Requests\n",
                "\t2. Display Stock Requests (True/False)\n",
                "\t3. Display All Product Lines\n",
                "\t4. Return to Main Menu\n",
                "\t5. Exit"};
            createMenu(menuLength, mainMenu);
            selectOption();
        }

        private static void selectOption()
        {
            Console.WriteLine("Enter an option:");
            string input = Console.ReadLine();
            switch (input)
            {
                case ("1"):
                    DisplayStockRequest();
                    break;
                case ("2"):
                    DisplayStockRequestTrueFalse();
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
                    selectOption();
                    break;
            }
        }

        public static void DisplayStock()
        {
            Console.Clear();
            Console.WriteLine("Stock Requests\n");
            Console.Write("ID".PadRight(5));
            Console.Write("Store".PadRight(25));
            Console.WriteLine("Current Stock");
            Console.WriteLine("----------------------------------------------");
            foreach (Owner owner1 in owner)
            {
                Console.WriteLine("{0}{1}{2}", owner1.Id.ToString().PadRight(5), owner1.Name.PadRight(25), owner1.StockLevel);
            }
            Console.WriteLine("Press Enter to return");
            Console.ReadLine();
            RunOwner();
        }

        public static void DisplayStockRequest()
        {
            List<StockRequests> requests = LoadStockRequest();
            Console.Clear();
            Console.WriteLine("Stock Requests\n");
            Console.Write("ID".PadRight(5));
            Console.Write("Store".PadRight(10));
            Console.Write("Product".PadRight(25));
            Console.Write("Quantity".PadRight(10));
            Console.Write("Current Stock".PadRight(15));
            Console.WriteLine("Stock Availability");
            Console.WriteLine("------------------------------------------------------------------------------------");
            for (int x = 0; x <= requests.Count - 1; x = x + 1)
            {
                foreach (Owner owner1 in owner)
                {

                    if (requests[x].Name == owner1.Name)
                    {
                        requests[x].StockLevel = owner1.StockLevel;
                    }
                }
                if (requests[x].Quantity <= requests[x].StockLevel)
                {
                    requests[x].Available = true;
                    Console.WriteLine("{0}{1}{2}{3}{4}{5}", requests[x].Id.ToString().PadRight(5), requests[x].Store.PadRight(10), requests[x].Name.PadRight(25), requests[x].Quantity.ToString().PadRight(10), requests[x].StockLevel.ToString().PadRight(15), requests[x].Available);
                }
                else
                {
                    requests[x].Available = false;
                    Console.WriteLine("{0}{1}{2}{3}{4}{5}", requests[x].Id.ToString().PadRight(5), requests[x].Store.PadRight(10), requests[x].Name.PadRight(25), requests[x].Quantity.ToString().PadRight(10), requests[x].StockLevel.ToString().PadRight(15), requests[x].Available);
                }
            }
            File.WriteAllText("stockrequests.json", JsonConvert.SerializeObject(requests, Formatting.Indented));
            ProcessOrder();
            DisplayStockRequest();
        }

        public static void ProcessOrder()
        {
            List<StockRequests> requests = LoadStockRequest();
            Console.WriteLine("\nEnter Request to process (press Q to go back)");
            string inputOrderToProcess = Console.ReadLine();

            // exit back to owner menu
            if (inputOrderToProcess == "q")
            {
                RunOwner();
            }

            // check to see if user input is in range of requests index
            int index = 0;
            bool isNumeric = int.TryParse(inputOrderToProcess,out index);
            if (isNumeric == true && index <= requests.Count)
            {
                index = Convert.ToInt32(inputOrderToProcess) - 1;
            }
            else
            {
                Console.WriteLine("Invaild option");
                ProcessOrder();
            }
            
            StockRequests processObject = requests[index];
            store = JsonConvert.DeserializeObject<List<Franchisee>>(File.ReadAllText(processObject.Store + "_inventory.json"));
            bool triggered = false;

            for (int x = 0; x <= store.Count - 1; x = x + 1)
            {
                if (store[x].Name == processObject.Name && processObject.Available == true)
                {
                    triggered = true;
                    string storeName = processObject.Store;
                    store[x].StockLevel += requests[index].Quantity;
                    removeStockOwner(processObject);
                    requests.RemoveAt(index);
                    File.WriteAllText("stockrequests.json", JsonConvert.SerializeObject(requests, Formatting.Indented));
                    File.WriteAllText(storeName + "_inventory.json", JsonConvert.SerializeObject(store, Formatting.Indented));
                    reindexStockRequests(requests);
                }else if (processObject.Available == false)
                {
                    Console.WriteLine("Not enough stock to process order");
                    ProcessOrder();
                }
            }

            if (triggered == false)
            {
                Console.WriteLine("Invaild option");
                ProcessOrder();
            }
        }

        public static void ProcessOrderTrue()
        {
            List<StockRequests> requests = JsonConvert.DeserializeObject<List<StockRequests>>(File.ReadAllText("stockrequeststrue.json"));
            Console.WriteLine("\nEnter Request to process (press Q to go back)");
            string inputOrderToProcess = Console.ReadLine();

            // exit back to owner menu
            if (inputOrderToProcess == "q")
            {
                RunOwner();
            }

            // check to see if user input is in range of requests index
            int index = 0;
            bool isNumeric = int.TryParse(inputOrderToProcess, out index);
            if (isNumeric == true && index <= requests.Count)
            {
                index = Convert.ToInt32(inputOrderToProcess) - 1;
            }
            else
            {
                Console.WriteLine("Invaild option");
                ProcessOrder();
            }

            StockRequests processObject = requests[index];
            store = JsonConvert.DeserializeObject<List<Franchisee>>(File.ReadAllText(processObject.Store + "_inventory.json"));
            bool triggered = false;

            for (int x = 0; x <= store.Count - 1; x = x + 1)
            {
                if (store[x].Name == processObject.Name)
                {
                    triggered = true;
                    string storeName = processObject.Store;
                    store[x].StockLevel += requests[index].Quantity;
                    removeStockOwner(processObject);
                    requests.RemoveAt(index);
                    File.WriteAllText("stockrequeststrue.json", JsonConvert.SerializeObject(requests, Formatting.Indented));
                    File.WriteAllText(storeName + "_inventory.json", JsonConvert.SerializeObject(store, Formatting.Indented));
                    reindexStockRequestsTrue(requests);
                }
            }

            if (triggered == false)
            {
                Console.WriteLine("Invaild option");
                ProcessOrder();
            }
        }

        public static void DisplayStockRequestTrueFalse()
        {
            List<StockRequests> requests = LoadStockRequest();
            Console.WriteLine("Enter True or False (Press Q to go back)");
            string input = Console.ReadLine();

            if (input == "true" || input == "T" || input == "True" || input == "TRUE")
            {
                int x3 = 0;
                List<StockRequests> requestsTrue = JsonConvert.DeserializeObject<List<StockRequests>>(File.ReadAllText("stockrequeststrue.json"));
                requestsTrue.Clear();
                Console.Clear();
                Console.WriteLine("\nStock Requests\n");
                Console.Write("ID".PadRight(5));
                Console.Write("Store".PadRight(10));
                Console.Write("Product".PadRight(25));
                Console.Write("Quantity".PadRight(10));
                Console.Write("Current Stock".PadRight(15));
                Console.WriteLine("Stock Availability");
                Console.WriteLine("------------------------------------------------------------------------------------");
                for (int x = 0; x <= requests.Count - 1; x = x + 1)
                {
                    if (requests[x].Available == true)
                    {
                        Console.WriteLine("{0}{1}{2}{3}{4}{5}",
                            (x3 + 1).ToString().PadRight(5),
                            requests[x].Store.PadRight(10),
                            requests[x].Name.PadRight(25),
                            requests[x].Quantity.ToString().PadRight(10),
                            requests[x].StockLevel.ToString().PadRight(15),
                            requests[x].Available);
                        requestsTrue.Add(new StockRequests() { Id = x3 + 1,
                            Name = requests[x].Name,
                            Store = requests[x].Store,
                            Quantity = requests[x].Quantity,
                            StockLevel = requests[x].StockLevel,
                            Available = requests[x].Available});
                        x3 = x3 + 1;
                    }
                }
                File.WriteAllText("stockrequeststrue.json", JsonConvert.SerializeObject(requestsTrue, Formatting.Indented));
                ProcessOrderTrue();
                DisplayStockRequestTrueFalse();
            }
            else if (input == "false" || input == "F" || input == "False" || input == "FALSE")
            {
                Console.Clear();
                Console.WriteLine("\nStock Requests\n");
                Console.Write("ID".PadRight(5));
                Console.Write("Store".PadRight(10));
                Console.Write("Product".PadRight(25));
                Console.Write("Quantity".PadRight(10));
                Console.Write("Current Stock".PadRight(15));
                Console.WriteLine("Stock Availability");
                Console.WriteLine("------------------------------------------------------------------------------------");
                for (int x = 0; x <= requests.Count - 1; x = x + 1)
                {
                    if (requests[x].Available == false)
                    {
                        int x2 = x + 1;
                        Console.WriteLine("{0}{1}{2}{3}{4}{5}", x2.ToString().PadRight(5), requests[x].Store.PadRight(10), requests[x].Name.PadRight(25), requests[x].Quantity.ToString().PadRight(10), requests[x].StockLevel.ToString().PadRight(15), requests[x].Available);
                    }
                }
            
                Console.WriteLine("\nCan not process any of these orders press Enter to go back");
                Console.ReadLine();
                RunOwner();
            }
            else if (input == "q")
            {
                RunOwner();
            }
            else
            {
                Console.WriteLine("Invaild option");
                DisplayStockRequestTrueFalse();
            }
        }
    }
}
