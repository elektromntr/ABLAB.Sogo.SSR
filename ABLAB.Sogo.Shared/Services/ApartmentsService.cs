using ABLAB.Sogo.Shared.Dtos;
using System.Net.Http.Json;

namespace ABLAB.Sogo.Shared.Services;

public class ApartmentsService
{
    private const decimal DefaultMargin = 0.18m;

    private readonly HttpClient _httpClient;
    
    public ApartmentsParams? SelectedSearchParameters { get; set; } = null!;
    public IList<ApartmentDto> ApartmentsStore { get; set; }


    public ApartmentsService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:56861/api/2/")
        };
    }

    public async Task<IList<ApartmentDto>> GetApartments(SearchParams? searchParams)
    {
        if (ApartmentsStore is null || ApartmentsStore.Count == 0)
        {
            await FetchApartmentsStore();
            if (ApartmentsStore is null || ApartmentsStore.Count == 0)
            {
                throw new Exception("Fetching data for ApartmentsStore failed");
            }
        }
        var result = ApartmentsStore.Where(a => (searchParams.SelectedMinPrice == null || a.Price >= searchParams.SelectedMinPrice)
                    && (searchParams.SelectedMaxPrice == null || a.Price <= searchParams.SelectedMaxPrice)
                    && (searchParams.SelectedRooms == null || a.Rooms == searchParams.SelectedRooms)
                    && (searchParams.SelectedArea == null
                        || a.Area >= (searchParams.SelectedArea - (searchParams.SelectedArea * DefaultMargin)) 
                        && a.Area <= (searchParams.SelectedArea + (searchParams.SelectedArea * DefaultMargin))))
                .Select(a => new ApartmentDto
                {
                    Id = a.Id,
                    Symbol = a.Symbol,
                    Investment = new InvestmentDto()
                    {
                        Id = a.Investment.Id,
                        Name = a.Investment.Name
                    },
                    Price = a.Price,
                    Area = a.Area,
                    Garden = a.Garden,
                    Rooms = a.Rooms,
                    Level = a.Level,
                    FinaleDate = a.FinaleDate,
                    Status = new StatusDto() { Id = a.Status.Id, Name = a.Status.Name },
                    Headlite = a.Headlite
                }).ToList();
        return result;
    }

    private async Task FetchApartmentsStore()
    {
        var toStore = await _httpClient.GetFromJsonAsync<IList<ApartmentDto>>("search", CancellationToken.None);
        if (toStore is not null && toStore.Count > 0)
        {
            ApartmentsStore = toStore;
        }
    }
}
