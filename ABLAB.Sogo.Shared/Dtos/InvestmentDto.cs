namespace ABLAB.Sogo.Shared.Dtos;

public class InvestmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IList<Description> Descriptions { get; set; } = Array.Empty<Description>();
}

public readonly struct Description
{
    public string Header { get; init; }
    public string Content { get; init; }
}