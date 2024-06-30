using ABLAB.Sogo.Shared.Dtos;
using ABLAB.Sogo.Shared.Services;
using Microsoft.AspNetCore.Components;
using System.Data;

namespace ABLAB.Sogo.SSR.Components;

public partial class PopularApartmentsComponent
{
    [Inject] private JsConsole JsConsole { get; set; } = default!;
    [Inject] private ApartmentsService ApartmentsService { get; set; } = default!;

    public IList<ApartmentDto>? PopularApartments { get; set; }

    protected override async Task OnInitializedAsync()
    {
        PopularApartments = await ApartmentsService.GetPopularApartments();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            
            await JsConsole.LogAsync("First render");
            await JsConsole.LogAsync("Popular apartments count: " + ApartmentsService.Popular.Count);
        }
        else
        {
            await JsConsole.LogAsync("NOT first render");
            await JsConsole.LogAsync("Popular apartments count: " + ApartmentsService.Popular.Count);
        }
    }
}
 = 