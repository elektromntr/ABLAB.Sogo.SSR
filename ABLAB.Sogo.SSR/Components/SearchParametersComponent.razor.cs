using ABLAB.Sogo.Shared.Dtos;
using ABLAB.Sogo.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace ABLAB.Sogo.SSR.Components;

public partial class SearchParametersComponent
{
    [Inject] private JsConsole JsConsole { get; set; } = default!;
    [Inject] private MenusService MenusService { get; set; } = default!;
    [Inject] private ApartmentsService ApartmentsService { get; set; } = default!;

    public decimal SelectedMaxPrice { get; set; }
    public decimal SelectedMinPrice { get; set; }
    public decimal SelectedRooms { get; set; }
    public decimal SelectedArea { get; set; }
    public decimal InvestmentId { get; set; }

    public ApartmentsParams? ApartmentsParams { get; set; }

    public SearchParametersComponent()
    {
            
    }

    protected override async Task OnInitializedAsync()
    {
        if (ApartmentsParams == null)
        {
            ApartmentsParams = await this.MenusService.GetMainFiltersAsync();
            SelectedMaxPrice = ApartmentsParams.MaxPrice!.Value;
        }
    }

    private async Task SetSearchParameters()
    {
        await JsConsole.LogAsync("Search button clicked");

        var searchParams = new SearchParams
        {
            SelectedMaxPrice = SelectedMaxPrice,
            SelectedMinPrice = SelectedMinPrice,
            SelectedRooms = SelectedRooms,
            SelectedArea = SelectedArea,
            InvestmentId = 0
        };

        var apartmnts = await this.ApartmentsService.GetApartments(searchParams);
    }

}
