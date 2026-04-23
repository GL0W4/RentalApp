using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.Views;

public partial class ItemDetailPage : ContentPage, IQueryAttributable
{
    private readonly IItemService _itemService;

    public Item? Item { get; set; }

    public string PageTitle => Item?.Title ?? "Item Detail";

    public ItemDetailPage()
    {
        InitializeComponent();

        _itemService = Application.Current!.Handler!.MauiContext!.Services.GetService(typeof(IItemService)) as IItemService
            ?? throw new InvalidOperationException("IItemService not found.");

        BindingContext = this;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("itemId", out var value) &&
            int.TryParse(value?.ToString(), out var itemId))
        {
            var items = await _itemService.GetItemsAsync();
            Item = items.FirstOrDefault(i => i.Id == itemId);

            OnPropertyChanged(nameof(Item));
            OnPropertyChanged(nameof(PageTitle));
        }
    }
}