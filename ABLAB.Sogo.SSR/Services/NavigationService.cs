using Microsoft.AspNetCore.Components;

namespace ABLAB.Sogo.SSR.Services;

public class NavigationService
{
    private readonly NavigationManager _navManager;

    public NavigationService(NavigationManager navigationManager)
    {
        _navManager = navigationManager;
    }

    public void NavigateToApartment(int apartmentId)
    {
        _navManager.NavigateTo($"/apartment/{apartmentId}");
    }

    public void NavigateToInvestment(int investmentId)
    {
        _navManager.NavigateTo($"/investment/{investmentId}");
    }

    public void NavigateHome()
    {
        _navManager.NavigateTo("/", true);
    }
}