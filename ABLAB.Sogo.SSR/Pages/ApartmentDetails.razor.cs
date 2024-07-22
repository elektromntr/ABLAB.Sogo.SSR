using ABLAB.Sogo.Shared.Dtos;
using ABLAB.Sogo.Shared.Services;
using ABLAB.Sogo.SSR.Services;
using Microsoft.AspNetCore.Components;

namespace ABLAB.Sogo.SSR.Pages;

public partial class ApartmentDetails : ComponentBase
{
    [Inject] private ApartmentsDetailsService ApartmentDetailsService { get; set; } = default!;
    [Inject] private NavigationService NavManager { get; set; } = default!;

    [Parameter] public int Id { get; set; }

    protected ApartmentDetailsDto Apartment { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        Apartment = await ApartmentDetailsService.GetApartmentDetails(Id);
        if (Apartment.Id == 0)
            throw new Exception("Apartment not found");
    }
}
