using ABLAB.Sogo.Shared.Configuration;
using ABLAB.Sogo.Shared.Dtos;
using ABLAB.Sogo.Shared.Extensions;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace ABLAB.Sogo.Shared.Services;

public class MenusService
{
    private const double DefaultCacheHours = 1;

    private ApartmentsParams? _searchParameters;
    private DateTime _lastUpdate;
    private readonly HttpClient _httpClient;
    private readonly IOptions<ApiUrls> _apiUrls = default!;

    public MenusService(IOptions<ApiUrls> apiUrls)
    {
        _apiUrls = apiUrls;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri($"{_apiUrls.Value.BaseUrl}/api/2/")
        };
    }

    public async Task<ApartmentsParams> GetMainFiltersAsync()
    {
        if (_searchParameters is null)
        {
            _searchParameters = await _httpClient.GetFromJsonAsync<ApartmentsParams>("search-params", CancellationToken.None);
            _lastUpdate = DateTime.Now;
        }
        else if (CacheHelper.CacheOutOfDate(
            _lastUpdate, TimeSpan.FromHours(DefaultCacheHours)))
        {
            try
            {
                _searchParameters = await _httpClient.GetFromJsonAsync<ApartmentsParams>("search-params", CancellationToken.None);
                _lastUpdate = DateTime.Now;
            }
            catch (Exception)
            {
                return _searchParameters!;
            }
        }
        return _searchParameters!;
    }
}
