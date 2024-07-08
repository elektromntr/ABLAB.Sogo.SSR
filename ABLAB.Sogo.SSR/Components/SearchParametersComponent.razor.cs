using ABLAB.Sogo.Shared.Dtos;
using ABLAB.Sogo.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace ABLAB.Sogo.SSR.Components;

public partial class SearchParametersComponent
{
    [Inject] private JsConsole JsConsole { get; set; } = default!;
    [Inject] private MenusService MenusService { get; set; } = default!;
    [Inject] private ApartmentsStore ApartmentsService { get; set; } = default!;
    [Inject] private EventsService EventsService { get; set; } = default!;

    protected const int DefaultPriceMargin = 100000;
    protected const decimal DefaultMargin = 0.2m;

    public decimal SelectedMaxPrice { get; set; }
    public decimal SelectedMinPrice { get; set; }
    public decimal SelectedRooms { get; set; }
    public decimal SelectedArea { get; set; }
    public int InvestmentId { get; set; }

    public ApartmentsParams ApartmentsParams { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        if (!ApartmentsParams.HasValue)
        {
            ApartmentsParams = await this.MenusService.GetMainFiltersAsync();
            SelectedMaxPrice = 0;
            SelectedMinPrice = 0;
            SelectedRooms = 0;
            SelectedArea = 0;
            InvestmentId = 0;
        }
    }

    private async Task FilterParameters()
    {
        await JsConsole.LogAsync("Search button clicked");

        var searchParams = new SearchParams
        {
            SelectedMaxPrice = this.SelectedMaxPrice,
            SelectedMinPrice = this.SelectedMinPrice,
            SelectedRooms = this.SelectedRooms,
            SelectedArea = this.SelectedArea,
            InvestmentId = this.InvestmentId
        };

        // Call the event with searchParams as arguments
        EventsService.OnFilterChanged(searchParams);
    }
}
