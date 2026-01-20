namespace HorusV2.Core.Helpers;

public class RequestContextStorage<TItemToStorage> where TItemToStorage : class, new()
{
    private TItemToStorage? _itemStored;

    public void Set(TItemToStorage itemToStorage)
    {
        _itemStored = itemToStorage;
    }

    public TItemToStorage Get()
    {
        return _itemStored ??= new TItemToStorage();
    }
}