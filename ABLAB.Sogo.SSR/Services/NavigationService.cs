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
        _navManager.NavigateTo($"/apartment/{apartmentId}", true);
    }

    public void NavigateToInvestment(int investmentId)
    {
        /* not ready yet
         * _navManager.NavigateTo($"/investment/{investmentId}");
         */
        NavigateHome();
    }

    public void NavigateHome()
    {
        _navManager.NavigateTo("/", true);
    }

    public void NavigateToContact()
    {
        /* not ready yet
         * _navManager.NavigateTo("/contact");
         */
        NavigateHome();
    }
}