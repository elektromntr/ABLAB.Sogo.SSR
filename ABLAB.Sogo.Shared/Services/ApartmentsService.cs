﻿using ABLAB.Sogo.Shared.Dtos;
using ABLAB.Sogo.Shared.Extensions;
using System.Net.Http.Json;

namespace ABLAB.Sogo.Shared.Services;

public class ApartmentsService
{
    private const decimal DefaultMargin = 0.18m;
    private const int DefaultPopularCount = 4;
    private const double DefaultCacheHours = 0.25;
    private readonly HttpClient _httpClient;

    private DateTime _lastUpdate;

    public IList<ApartmentDto> Store { get; set; } = Array.Empty<ApartmentDto>();
    public IList<ApartmentDto> Filtered { get; set; } = Array.Empty<ApartmentDto>();
    public IList<ApartmentDto> Popular { get; set; } = Array.Empty<ApartmentDto>();

    public event Action OnChange;

    public ApartmentsService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:56861/api/2/")
        };
    }

    private void NotifyStateChanged() => OnChange?.Invoke();

    public async Task FilterApartments(SearchParams? searchParams)
    {
        await CheckStore();
        if (searchParams.SelectedMinPrice > searchParams.SelectedMaxPrice && searchParams.SelectedMaxPrice != 0)
        {
            searchParams.SelectedMinPrice = 0;
        }
        else if (searchParams.SelectedMaxPrice == searchParams.SelectedMinPrice && searchParams.SelectedMinPrice == 0)
        {
            searchParams.SelectedMaxPrice = decimal.MaxValue;
        }
        var result = Store.Where(a => (searchParams.SelectedMinPrice == null || a.Price >= searchParams.SelectedMinPrice)
                    && (searchParams.SelectedMaxPrice == null || a.Price <= searchParams.SelectedMaxPrice)
                    && (searchParams.SelectedRooms == null
                        || (searchParams.SelectedRooms == 0 ? a.Rooms > 0 : a.Rooms == searchParams.SelectedRooms))
                    && (searchParams.SelectedArea == null
                        || searchParams.SelectedArea == 0
                        ? a.Area > 0
                        : a.Area >= (searchParams.SelectedArea - (searchParams.SelectedArea * DefaultMargin))
                            && a.Area <= (searchParams.SelectedArea + (searchParams.SelectedArea * DefaultMargin))))
                //.Select(a => new ApartmentDto
                //{
                //    Id = a.Id,
                //    Symbol = a.Symbol,
                //    Investment = new InvestmentDto()
                //    {
                //        Id = a.Investment.Id,
                //        Name = a.Investment.Name
                //    },
                //    Price = a.Price,
                //    Area = a.Area,
                //    Garden = a.Garden,
                //    Rooms = a.Rooms,
                //    Level = a.Level,
                //    FinaleDate = a.FinaleDate,
                //    Status = new StatusDto() { Id = a.Status.Id, Name = a.Status.Name },
                //    Headlite = a.Headlite})
                .ToList();

        Filtered = result;
    }

    public async Task SetPopularApartments()
     {
        if (Popular.Count > 0)
        {
            NotifyStateChanged();
            return;
        }

        await CheckStore();

        var popular = Store.OrderByDescending(a => a.Counter)
            .Take(DefaultPopularCount).ToList();

        if (popular is not null && popular.Count > 0)
        {
            Popular = popular;
            NotifyStateChanged();
        }
    }
    
    public async Task<IList<ApartmentDto>> GetPopularApartments()
    {
        await CheckStore();
        
        var popular = Store.OrderByDescending(a => a.Counter)
            .Take(DefaultPopularCount).ToList();

        if (popular is not null && popular.Count > 0)
        {
            Popular = popular;
        }


        return Popular;
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
        var toStore = await _httpClient.GetFromJsonAsync<IList<ApartmentDto>>("search", CancellationToken.None);
        if (toStore is not null && toStore.Count > 0)
        {
            Store = toStore;
            _lastUpdate = DateTime.Now;
        }
    }
}
