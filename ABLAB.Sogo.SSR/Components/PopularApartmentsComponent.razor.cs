using ABLAB.Sogo.Shared.Dtos;
using ABLAB.Sogo.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ABLAB.Sogo.SSR.Components;

public partial class PopularApartmentsComponent : ComponentBase
{
    [Inject] private JsConsole JsConsole { get; set; } = default!;
    [Inject] private IJSRuntime JS { get; set; } = default!;
    [Inject] private ApartmentsStore ApartmentsService { get; set; } = default!;

    public IList<ApartmentDto> PopularApartments { get; set; } = Array.Empty<ApartmentDto>();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            PopularApartments = await ApartmentsService.GetPopularApartments(0, 4);
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
            await JS.InvokeVoidAsync("initializePopularApartmentsComponentOwlCarousel");
            await JsConsole.LogAsync("First render");
            await JsConsole.LogAsync("Popular apartments count: " + PopularApartments.Count);
        }
        else
        {
            await JsConsole.LogAsync("NOT first render");
            await JsConsole.LogAsync("Popular apartments count: " + PopularApartments.Count);
        }
    }
}
