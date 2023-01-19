using BikeServiceCenter.Pages;
using System.Data;
using System.Text.Json;
namespace BikeServiceCenter.Data;
public static class UserService
{
    // default users while running the system
    public const string SeedUsername = "admin";
    public const string SeedPassword = "admin";

    private static void SaveAll(List<User> users)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string appUsersFilePath = Utils.GetAppUsersFilePath();
        //create a new directory if there is no directory present
        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(users);
        File.WriteAllText(appUsersFilePath, json);
    }

    public static List<User> Create(Guid userId, string username, string password, string role)
    {
        List<User> users = GetAll(); //getting all users
        bool usernameExists = users.Any(x => x.Username == username);
        //add user
        users.Add(
            new User
            {
                Username = username,
                Password = password,
                Role = role,
                CreatedDate = DateTime.Now,
            }
        );
        SaveAll(users);
        return users;
    }

    public static void SeedUsers()
    {
        var users = GetAll().FirstOrDefault(x => x.Role == "Admin");

        if (users == null)
        {
            Create(Guid.Empty, SeedUsername, SeedPassword, "Admin");
        }
    }
    public static bool ValidateCredentials(string username,string password)
    {

        //read list from json file
        string filepath = Utils.GetAppUsersFilePath();
        string json = File.ReadAllText(filepath);
        var credentials = JsonSerializer.Deserialize<List<User>>(json);
        //check if the username and password match
        return credentials.Any(c=>c.Username == username && c.Password == password);

    }

    public static List<User> GetAll()
    {
        string appUsersFilePath = Utils.GetAppUsersFilePath();
        if (!File.Exists(appUsersFilePath))
        {
            return new List<User>();
        }

        var json = File.ReadAllText(appUsersFilePath);

        return JsonSerializer.Deserialize<List<User>>(json);
    }

    public static bool AddAdmin(string name,string password,string CreatedBy)
    {
        string filepath = Utils.GetAppUsersFilePath();
        //read list from json file
        string json = File.ReadAllText(filepath);
        var admins = JsonSerializer.Deserialize<List<User>>(json);
        //condition to check if the name and password is null
        if(name == null || password == null)
        {
            return false;
        }
        //condition to check if there are already 2 admins present
        if(admins.Count >= 2)
        {
            return false;
        }
        string role = "Admin";
        DateTime createdAt = DateTime.Now;
        //adding new user
        admins.Add(new User()
        {
            Username = name,
            Password = password,
            Role = role,
            CreatedDate = createdAt,
            CreatedBy = CreatedBy
        });
        json = JsonSerializer.Serialize(admins);
        File.WriteAllText(filepath, json);
        return true;
    }

    public static bool Delete(Guid id) 
    {
        
        string filepath = Utils.GetAppUsersFilePath();
        //read list from json file
        string json = File.ReadAllText(filepath);
        var admins = JsonSerializer.Deserialize<List<User>>(json);
        //condition to check if there is only 1 admin present
        if (admins.Count == 1)
        {
            return false;
        }
        //remove admin with the specific id
        admins.RemoveAll(x => x.Id == id);
        //write the updated list to the json file
        json = JsonSerializer.Serialize(admins);
        File.WriteAllText(filepath, json);
        return true;

    }
}