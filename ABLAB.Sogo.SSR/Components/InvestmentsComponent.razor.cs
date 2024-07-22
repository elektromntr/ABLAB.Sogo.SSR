using ABLAB.Sogo.Shared.Dtos;
using ABLAB.Sogo.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace ABLAB.Sogo.SSR.Components;

public partial class InvestmentsComponent : ComponentBase
{
    [Inject] private ApartmentsStore ApartmentsStore { get; set; } = default!;

    protected IList<InvestmentDto> Investments { get; set; } = Array.Empty<InvestmentDto>();

    protected async Task GetInvestments()
    {
        var investements = await ApartmentsStore.GetInvestments();

        Investments = investements;
    }

    protected override async Task OnInitializedAsync()
    {
        await GetInvestments();
    }
}
