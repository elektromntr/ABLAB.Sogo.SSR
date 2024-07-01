using ABLAB.Sogo.Shared.Dtos;
using ABLAB.Sogo.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;

namespace ABLAB.Sogo.SSR.Components;

public partial class FilteredApartmentsComponent
{
    [Inject] private JsConsole JsConsole { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;
    [Inject] private ApartmentsService ApartmentsService { get; set; } = default!;

    protected int TakeCount { get; set; } = 6;

    public IList<ApartmentDto> FilteredApartments { get; set; } = Array.Empty<ApartmentDto>();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ApartmentsService.FilterChanged += HandleFilterChanged;
            FilteredApartments = await ApartmentsService.GetFilteredApartments(ApartmentsService.Filter);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"EXCEPTION; {e}");
        }
    }

    private async void HandleFilterChanged()
    {
        FilteredApartments = await ApartmentsService.GetFilteredApartments(ApartmentsService.Filter);
        await InvokeAsync(StateHasChanged); // Trigger a re-render
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            //await JS.InvokeVoidAsync("initializeFilteredApartmentsMixItUpGallery", ".filter-list");
            await JsConsole.LogAsync("First render");
            await JsConsole.LogAsync("Filtered apartments count: " + FilteredApartments.Count);
        }
        else
        {
            await JsConsole.LogAsync("NOT first render");
            await JsConsole.LogAsync("Filtered apartments count: " + FilteredApartments.Count);
        }
    }
}
