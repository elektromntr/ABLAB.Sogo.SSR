namespace ABLAB.Sogo.Shared.Dtos;

public class SearchParams
{
    public decimal SelectedMaxPrice { get; set; } = decimal.MaxValue;
    public decimal SelectedMinPrice { get; set; }
    public decimal SelectedRooms { get; set; }
    public decimal SelectedArea { get; set; }
    public int InvestmentId { get; set; }
}
