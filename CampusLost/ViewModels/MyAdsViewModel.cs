using CampusLost.Models;
using CampusLost.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CampusLost.ViewModels;

public partial class MyAdsViewModel : ObservableObject
{
    readonly DataService data;
    public MyAdsViewModel(DataService data) => this.data = data;

    [ObservableProperty] private List<Item> active = new();
    [ObservableProperty] private List<Item> resolved = new();

    [ObservableProperty, NotifyPropertyChangedFor(nameof(ShowActive), nameof(ShowResolved))]
    private string tab = "active";

    [ObservableProperty] private string activeTabText = "";
    [ObservableProperty] private string resolvedTabText = "";

    public bool ShowActive => Tab == "active";
    public bool ShowResolved => Tab == "resolved";

    public void Refresh()
    {
        Active = data.Mine(false);
        Resolved = data.Mine(true);
        ActiveTabText = $"En cours ({Active.Count})";
        ResolvedTabText = $"Résolues ({Resolved.Count})";
    }

    [RelayCommand] void SetTab(string t) => Tab = t;
    [RelayCommand] Task Create() => Shell.Current.GoToAsync("report?mode=lost");

    public Task Edit(Item item) => Shell.Current.GoToAsync($"report?edit={item.Id}");

    public async Task MarkResolved(Item item)
    {
        if (!await Shell.Current.DisplayAlert("Marquer comme résolu", $"« {item.Name} » a retrouvé son propriétaire ?", "Oui", "Non")) return;
        data.Resolve(item);
        Refresh();
    }
}
