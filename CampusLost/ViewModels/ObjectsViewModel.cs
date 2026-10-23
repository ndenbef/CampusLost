using CampusLost.Models;
using CampusLost.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CampusLost.ViewModels;

[QueryProperty(nameof(Query), "q")]
public partial class ObjectsViewModel : ObservableObject
{
    readonly DataService data;
    const int PageSize = 10;

    public List<string> Categories { get; } = new[] { DataService.AllCategories }.Concat(DataService.Categories).ToList();

    public ObjectsViewModel(DataService data)
    {
        this.data = data;
        Refresh();
    }

    [ObservableProperty] private string query = "";
    [ObservableProperty] private string filter = "all";   // all | lost | found
    [ObservableProperty] private string category = DataService.AllCategories;
    [ObservableProperty] private int visibleCount = PageSize;
    [ObservableProperty] private List<Item> items = new();
    [ObservableProperty] private string countText = "";
    [ObservableProperty] private bool canShowMore;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(SortText))]
    private bool newestFirst = true;

    public string SortText => NewestFirst ? "Plus récents" : "Plus anciens";

    partial void OnQueryChanged(string value) { VisibleCount = PageSize; Refresh(); }
    partial void OnFilterChanged(string value) { VisibleCount = PageSize; Refresh(); }
    partial void OnCategoryChanged(string value) { VisibleCount = PageSize; Refresh(); }
    partial void OnNewestFirstChanged(bool value) => Refresh();
    partial void OnVisibleCountChanged(int value) => Refresh();

    public void Refresh()
    {
        var all = data.Search(Query, Filter, Category ?? DataService.AllCategories, NewestFirst);
        CountText = $"{all.Count} objet{(all.Count > 1 ? "s" : "")} sur le campus";
        Items = all.Take(VisibleCount).ToList();
        CanShowMore = all.Count > VisibleCount;
    }

    [RelayCommand] void SetFilter(string f) => Filter = f;
    [RelayCommand] void ToggleSort() => NewestFirst = !NewestFirst;
    [RelayCommand] void ShowMore() => VisibleCount += PageSize;
}
