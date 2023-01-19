namespace BikeServiceCenter.Data;

public class WithdrawItem
{
    public string Name { get; set; }
    public int Quantity { get; set; }

    public string ApprovedBy { get; set; }
    public string TakenBy { get; set; }
    public DateTime DateTakenOut { get; set; }
}