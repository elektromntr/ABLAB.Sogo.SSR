using ABLAB.Sogo.Shared.Dtos;
using ABLAB.Sogo.Shared.Extensions;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ABLAB.Sogo.Shared.Services;

public class ApartmentsDetailsService
{
    private const int DefaultTopCount = 4;

    private readonly HttpClient _httpClient;
    private readonly OfficesService _officesService;
    private readonly ApartmentsStore _apartmentsStore;

    public static List<ApartmentDetailsDto> Store { get; set; } = new();

    public ApartmentsDetailsService(OfficesService officesService,
        ApartmentsStore apartmentsStore)
    {
        _officesService = officesService;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri($"{ApartmentsStore.ApiBaseUrl}/api/2/")
        };
        _apartmentsStore = apartmentsStore;
    }

    public async Task<ApartmentDetailsDto> GetApartmentDetails(int id)
    {
        var result = Store.FirstOrDefault(a => a.Id == id);
        if (result is null)
        {
            result = await FetchApartmentDetails(id);
        }
        else if (CacheHelper.CacheOutOfDate(result.LastUpdate))
        {
            result = await FetchApartmentDetails(id);
        }

        return result;
    }

    public async Task<ApartmentDto[]> GetTopApartments(int investmentId)
    {
        var result = await _apartmentsStore.GetPopularApartments(investmentId, DefaultTopCount);
        return result ?? Array.Empty<ApartmentDto>();
    }

    private async Task<ApartmentDetailsDto> FetchApartmentDetails(int id)
    {
        ApartmentDetailsDto? apartment = null;
        try
        {
            apartment = await _httpClient.GetFromJsonAsync<ApartmentDetailsDto>($"apartment/{id}", new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            },
            CancellationToken.None);
            if (apartment is not null)
            {
                apartment.LastUpdate = DateTime.Now;
                apartment.Investment.Office = _officesService.GetOffice(apartment.Investment.Id);
                Store.Add(apartment);
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"EXCEPTION; {e}");
        }
        return apartment ?? new();
    }
}
