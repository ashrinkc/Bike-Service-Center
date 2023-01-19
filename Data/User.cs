namespace BikeServiceCenter.Data;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = "Unknown";
}