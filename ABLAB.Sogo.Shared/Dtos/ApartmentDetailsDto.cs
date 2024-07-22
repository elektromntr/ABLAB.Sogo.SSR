namespace ABLAB.Sogo.Shared.Dtos;

public class ApartmentDetailsDto
{
    public int Id { get; set; }
    public string Symbol { get; set; }
    public InvestmentDto Investment { get; set; } = default!;
    public string Status { get; set; }
    public decimal Price { get; set; }
    public int? Level { get; set; }
    public int? Rooms { get; set; }
    public decimal Area { get; set; }
    public decimal? Garden { get; set; }
    public decimal? Entresol { get; set; }
    public decimal? Garage { get; set; }
    public bool? ParkingSlot { get; set; }
    public bool? Storage { get; set; }
    public DateTime FinaleDate { get; set; }
    public string ImageFile
    {
        get
        {
            return "/Rzuty/" + Symbol + ".jpg";
        }
        set
        {

        }
    }
    public string AdditionalInfo { get; set; } = string.Empty;
    public DateTime LastUpdate { get; set; }
    public bool IsFetched =>
        Id != 0
        && !string.IsNullOrWhiteSpace(Symbol)
        && Investment != null
        && !string.IsNullOrWhiteSpace(Investment.Name)
        && Rooms > 0;
}
