using ABLAB.Sogo.Shared.Configuration;
using ABLAB.Sogo.Shared.Dtos;
using ABLAB.Sogo.Shared.Extensions;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ABLAB.Sogo.Shared.Services;

public class ApartmentsDetailsService
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<ApiUrls> _apiUrls = default!;

    public List<ApartmentDetailsDto> Store { get; set; } = new();
    public static string ApiBaseUrl { get; set; } = default!;

    public ApartmentsDetailsService(IOptions<ApiUrls> apiUrls)
    {
        _apiUrls = apiUrls;
        ApiBaseUrl = _apiUrls.Value.BaseUrl;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri($"{_apiUrls.Value.BaseUrl}/api/2/")
        };
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
