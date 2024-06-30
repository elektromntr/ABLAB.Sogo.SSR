namespace ABLAB.Sogo.Shared.Dtos;

public class ApartmentsParams
{
    private decimal minArea;
    private decimal maxArea;

    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public int MinRooms { get; set; }
    public int MaxRooms { get; set; }
    public decimal MinArea 
    { 
        get => decimal.Round(minArea, 0, MidpointRounding.ToZero); 
        set => minArea = value; 
    }
    public decimal MaxArea 
    { 
        get => decimal.Round(maxArea, 0, MidpointRounding.AwayFromZero); 
        set => maxArea = value; 
    }
    public bool Garden { get; set; }
    public bool Storage { get; set; }
    public IList<InvestmentDto> Investments { get; set; } = Array.Empty<InvestmentDto>();

    public bool HasValue => 
        MinPrice != default &&
        MaxPrice != default &&
        MinRooms != default &&
        MaxRooms != default &&
        MinArea != default &&
        MaxArea != default &&
        Garden != default &&
        Storage != default && 
        Investments.Count > 0;
}
