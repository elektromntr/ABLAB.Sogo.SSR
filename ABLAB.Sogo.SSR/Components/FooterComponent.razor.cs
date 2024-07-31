using Microsoft.AspNetCore.Components;

namespace ABLAB.Sogo.SSR.Components;

public partial class FooterComponent : ComponentBase
{
    public int InvestmentId { get; set; }
    public string CurrentYear => DateTime.Now.Year.ToString();
}
