namespace BikeServiceCenter.Data;

public class Item
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public int Quantity { get; set; }

    public DateTime LastTakenOutOn { get; set; }
    public double TimesTakenOut { get; set; } = 0;
}