using ABLAB.Sogo.Shared.Dtos;

namespace ABLAB.Sogo.Shared.Services;

public class EventsService
{
    public event Action<SearchParams> FilterChanged = default!;

    public void OnFilterChanged(SearchParams filter)
    {
        FilterChanged.Invoke(filter);
    }
}
