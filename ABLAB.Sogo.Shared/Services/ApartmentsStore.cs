using ABLAB.Sogo.Shared.Configuration;
using ABLAB.Sogo.Shared.Dtos;
using ABLAB.Sogo.Shared.Extensions;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace ABLAB.Sogo.Shared.Services;

public class ApartmentsStore
{
    private const decimal DefaultMargin = 0.18m;
    private const int DefaultPopularCount = 3;
    private const double DefaultCacheHours = 0.25;
    
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    
    private readonly HttpClient _httpClient;
    private readonly IOptions<ApiUrls> _apiUrls = default!;
    
    private DateTime _lastUpdate;

    public IList<ApartmentDto> Store { get; set; } = Array.Empty<ApartmentDto>();
    public static string ApiBaseUrl { get; set; }

    public ApartmentsStore(IOptions<ApiUrls> apiUrls)
    {
        _apiUrls = apiUrls;
        ApiBaseUrl = _apiUrls.Value.BaseUrl;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri($"{_apiUrls.Value.BaseUrl}/api/2/")
        };
    }

    public async Task<IList<ApartmentDto>> GetFilteredApartments(SearchParams searchParams)
    {
        await CheckStore();

        if (searchParams.SelectedMinPrice > searchParams.SelectedMaxPrice && searchParams.SelectedMaxPrice != 0)
        {
            searchParams.SelectedMinPrice = 0;
        }
        else if (searchParams.SelectedMaxPrice == 0 && searchParams.SelectedMinPrice != 0)
        {
            searchParams.SelectedMaxPrice = decimal.MaxValue;
        }
        else if (searchParams.SelectedMaxPrice == 0 && searchParams.SelectedMinPrice == 0)
        {
            searchParams.SelectedMaxPrice = decimal.MaxValue;
        }
        var result = Store.Where(a => (searchParams.InvestmentId == 0
                ? a.Investment.Id > 0
                : a.Investment.Id == searchParams.InvestmentId)
                && (string.IsNullOrEmpty(searchParams.StatusName)
                    ? a.Status.Name != string.Empty
                    : a.Status.Name == searchParams.StatusName)
                && (a.Price >= searchParams.SelectedMinPrice)
                && (a.Price <= searchParams.SelectedMaxPrice)
                && ((searchParams.SelectedRooms == 0 ? a.Rooms > 0 : a.Rooms == searchParams.SelectedRooms))
                && (searchParams.SelectedArea == 0
                    ? a.Area > 0
                    : a.Area >= (searchParams.SelectedArea - (searchParams.SelectedArea * DefaultMargin))
                        && a.Area <= (searchParams.SelectedArea + (searchParams.SelectedArea * DefaultMargin))))
            .OrderByDescending(a => a.Counter)
            .ToList();

        return result;
    }

    public async Task<IList<ApartmentDto>> GetPopularApartments()
    {
        await CheckStore();

        var popular = Store.OrderByDescending(a => a.Counter)
            .Take(DefaultPopularCount).ToList();

        return (popular is not null && popular.Count > 0) ? popular : Array.Empty<ApartmentDto>();
    }

    public async Task<IList<InvestmentDto>> GetInvestments()
    {
        await CheckStore();
        var result = Store.Select(a => a.Investment).DistinctBy(d => d.Id).ToList();
        return result;
    }

    private async Task CheckStore()
    {
        await _semaphore.WaitAsync();
        try
        {
            if (Store is null || Store.Count == 0)
            {
                await FetchApartments();
                if (Store is null || Store.Count == 0)
                {
                    throw new Exception("Fetching data for Store failed");
                }
            }
            if (CacheHelper.CacheOutOfDate(_lastUpdate, TimeSpan.FromHours(DefaultCacheHours)))
            {
                await FetchApartments();
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task FetchApartments()
    {
        IList<ApartmentDto> toStore = null;
        try
        {
            toStore = await _httpClient.GetFromJsonAsync<IList<ApartmentDto>>("search", CancellationToken.None);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"EXCEPTION; {e}");
        }
        if (toStore is not null && toStore.Count > 0)
        {
            Store = toStore;
            _lastUpdate = DateTime.Now;
        }
    }
}
