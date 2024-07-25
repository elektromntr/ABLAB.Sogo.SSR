using ABLAB.Sogo.Shared.Dtos;
using ABLAB.Sogo.Shared.Services;
using ABLAB.Sogo.SSR.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ABLAB.Sogo.SSR.Components;

public partial class FilteredApartmentsComponent
{
    private const int DefaultTakeCount = 6;

    [Inject] private JsConsole JsConsole { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;
    [Inject] private ApartmentsStore ApartmentsService { get; set; } = default!;
    [Inject] private EventsService EventsService { get; set; } = default!;
    [Inject] private NavigationService NavManager { get; set; } = default!;

    protected int TakeCount { get; set; } = DefaultTakeCount;
    protected SearchParams Filter { get; set; } = new();
    protected string[] Statuses { get; set; } = Array.Empty<string>();


    public IList<ApartmentDto> FilteredApartments { get; set; } = Array.Empty<ApartmentDto>();


    protected async Task LoadMore()
    {
        TakeCount += 6;
        await InvokeAsync(StateHasChanged);
    }

    protected void FilterStatus(string statusName)
    {
        Filter.StatusName = statusName;
        HandleFilterChanged(Filter);
    }

    protected void GetStatuses()
    {
        var statuses = FilteredApartments
            .Where(s => s.Status.Name != "Sprzedane")
            .Select(a => a.Status.Name)
            .Distinct()
            .ToArray();
        if (statuses.Length > 0) Statuses = statuses;
    }

    protected string GetInvestmentThumb(int investmentId)
    {
        return "logoSogo-thumb.png";
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            EventsService.FilterChanged += HandleFilterChanged;
            FilteredApartments = await ApartmentsService.GetFilteredApartments(Filter);
            GetStatuses();
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"EXCEPTION; {e}");
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JsConsole.LogAsync("First render");
            await JsConsole.LogAsync("Filtered apartments count: " + FilteredApartments.Count);
        }
        else
        {
            await JsConsole.LogAsync("NOT first render");
            await JsConsole.LogAsync("Filtered apartments count: " + FilteredApartments.Count);
        }
    }


    private async void HandleFilterChanged(SearchParams filter)
    {
        filter ??= new SearchParams();
        Filter = filter;
        TakeCount = DefaultTakeCount;
        FilteredApartments = await ApartmentsService.GetFilteredApartments(filter);
        //GetStatuses();
        await InvokeAsync(StateHasChanged); // Trigger a re-render
    }
}
