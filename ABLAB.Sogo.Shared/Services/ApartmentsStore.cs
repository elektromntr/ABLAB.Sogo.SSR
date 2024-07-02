using ABLAB.Sogo.Shared.Dtos;
using ABLAB.Sogo.Shared.Extensions;
using System.Net.Http.Json;

namespace ABLAB.Sogo.Shared.Services;

public class ApartmentsStore
{
    public const string ApiBaseUrl = "http://localhost:56861";
    private const decimal DefaultMargin = 0.18m;
    private const int DefaultPopularCount = 3;
    private const double DefaultCacheHours = 0.25;
    private readonly HttpClient _httpClient;

    private DateTime _lastUpdate;

    public IList<ApartmentDto> Store { get; set; } = Array.Empty<ApartmentDto>();

    public ApartmentsStore()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri($"{ApiBaseUrl}/api/2/")
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
                && (a.Price >= searchParams.SelectedMinPrice)
                && (a.Price <= searchParams.SelectedMaxPrice)
                && ((searchParams.SelectedRooms == 0 ? a.Rooms > 0 : a.Rooms == searchParams.SelectedRooms))
                && (searchParams.SelectedArea == 0
                    ? a.Area > 0
                    : a.Area >= (searchParams.SelectedArea - (searchParams.SelectedArea * DefaultMargin))
                        && a.Area <= (searchParams.SelectedArea + (searchParams.SelectedArea * DefaultMargin))))
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

    private async Task CheckStore()
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
