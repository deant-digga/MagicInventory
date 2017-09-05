using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MagicInventory
{
    public class Menu
    {
        public static List<Franchisee> store;

        public static void createMenu(int menuItems, string[] menuOptions)
        {
            Console.Clear();

            for (int a = 0; a < menuItems; a = a + 1)
            {
                Console.WriteLine(menuOptions[a]);
            }

        }

        public static List<StockRequests> LoadStockRequest()
        {
            return JsonConvert.DeserializeObject<List<StockRequests>>(File.ReadAllText("stockrequests.json"));
        }

        public static List<Owner> LoadOwnerStock()
        {
            return JsonConvert.DeserializeObject<List<Owner>>(File.ReadAllText("owners_inventory.json"));
        }

        public static void reindexStockRequests(List<StockRequests> stockRequest)
        {
            for (int x = 0; x <= stockRequest.Count - 1; x = x + 1)
            {
                stockRequest[x].Id = x + 1;
            }
            File.WriteAllText("stockrequests.json", JsonConvert.SerializeObject(stockRequest, Formatting.Indented));
        }

        public static void reindexStockRequestsTrue(List<StockRequests> stockRequest)
        {
            for (int x = 0; x <= stockRequest.Count - 1; x = x + 1)
            {
                stockRequest[x].Id = x + 1;
            }
            File.WriteAllText("stockrequeststrue.json", JsonConvert.SerializeObject(stockRequest, Formatting.Indented));
        }

        public static void removeStockOwner(StockRequests request)
        {
            List<Owner> owner = LoadOwnerStock();
            for (int x = 0; x <= owner.Count - 1; x = x + 1)
            {
                if (owner[x].Name == request.Name)
                {
                    owner[x].StockLevel -= request.Quantity;
                    File.WriteAllText("owners_inventory.json", JsonConvert.SerializeObject(owner, Formatting.Indented));
                }
            }

        }
    }
}
