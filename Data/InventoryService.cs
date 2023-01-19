using System.Text.Json;

namespace BikeServiceCenter.Data;

public static class InventoryService
{
    public static List<Item> GetInventoryData()
    {
        
        string appItemFilePath = Utils.GetItemFilePath();
        if (!File.Exists(appItemFilePath))
        {
            //creating a default list of items and serializing it
            List<Item> inventoryData = new List<Item>
            {
                new Item{Name="Wheels",Quantity=10,TimesTakenOut=5},
            };
            string item = JsonSerializer.Serialize<List<Item>>(inventoryData);

            // Write the string to the file
            File.WriteAllText(appItemFilePath, item);

        }

        // Read the contents of the file into a string
        var json = File.ReadAllText(appItemFilePath);

            // Try to deserialize the string and return it
            return JsonSerializer.Deserialize<List<Item>>(json);
    
        
    }
    public static bool AddInventory(string itemname, int quantity)
    {
        //read the inventory
        string filepath = Utils.GetItemFilePath();
        var json = File.ReadAllText(filepath);
        var inventory = JsonSerializer.Deserialize<List<Item>>(json);
        bool found = false;
        foreach (var item in inventory)
        {
            if (item.Name == itemname)
            {
                item.Quantity += quantity;
                found = true;
                break;
            }
        }
            if (!found)
            {
                //add to the inventory
                inventory.Add(new Item()
                {
                    Name = itemname,
                    Quantity = quantity,
                });
            }
            json = JsonSerializer.Serialize(inventory);
            File.WriteAllText(filepath, json);
        return true;
    }

    public static void RemoveItemFromInventory(String itemName,int quantity)
    {
        string filepath = Utils.GetItemFilePath();
        string json = File.ReadAllText(filepath);
        var inventory = JsonSerializer.Deserialize<List<Item>>(json);
        Item item = inventory.FirstOrDefault(i => i.Name == itemName);

        if (item != null)
        {
            item.Quantity -= quantity;
            item.TimesTakenOut += quantity; // increasing the quantity taken out

            //saving the updated inventory to JSON file
            json = JsonSerializer.Serialize(inventory);
            File.WriteAllText(filepath, json);
        }

    }

    public static bool AddWithDrawItems(string itemName,int quantity,string takenBy,string approvedBy)
    {
        //read the inventory
        string filepath = Utils.GetWithDrawItemFilePath();
        var json = File.ReadAllText(filepath);
        var inventory = JsonSerializer.Deserialize<List<WithdrawItem>>(json);
        DateTime dateTakenOut = DateTime.Now;
        //add to the inventory
        inventory.Add(new WithdrawItem()
        {
            Name=itemName,
            Quantity=quantity,
            TakenBy=takenBy,
            DateTakenOut=dateTakenOut,
            ApprovedBy = approvedBy,
        });
        json = JsonSerializer.Serialize(inventory);
        File.WriteAllText(filepath, json);
        
        string filepath2 = Utils.GetItemFilePath();
        string json2 = File.ReadAllText(filepath2);
        var inventory2 = JsonSerializer.Deserialize<List<Item>>(json2);
        //initializing the value of the item
        Item item = inventory2.FirstOrDefault(i => i.Name == itemName);
        //updating the last taken out on date
        item.LastTakenOutOn = dateTakenOut;
        //saving the updated JSON file
        json2 = JsonSerializer.Serialize(inventory2);
        File.WriteAllText(filepath2, json2);
        return true;
    }

    public static List<WithdrawItem> GetWithDrawItem()
    {
        //read the inventory

        string appWithdrawItemFilePath = Utils.GetWithDrawItemFilePath();
        if (!File.Exists(appWithdrawItemFilePath))
        {
            //creating a default list of items and serializing it
            List<WithdrawItem> inventoryData = new List<WithdrawItem>
            {
                new WithdrawItem{Name="Wheels",Quantity=5,TakenBy="Ashrin",ApprovedBy="Admin",DateTakenOut=DateTime.Now},
            };
            string item = JsonSerializer.Serialize(inventoryData);

            // Write the string to the file
            File.WriteAllText(appWithdrawItemFilePath, item);

        }
        // Read the contents of the file into a string
        var json = File.ReadAllText(appWithdrawItemFilePath);
        return JsonSerializer.Deserialize<List<WithdrawItem>>(json);
    }

    public static bool Delete(Guid id)
    {
  
        string filepath = Utils.GetItemFilePath();
        //read list from json file
        string json = File.ReadAllText(filepath);
        var items = JsonSerializer.Deserialize<List<Item>>(json);
        //remove item with the specific id
        items.RemoveAll(x => x.Id == id);
        //write the updated list to the json file
        json = JsonSerializer.Serialize(items);
        File.WriteAllText(filepath, json);
        return true;
    }
}