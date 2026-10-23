using CampusLost.Models;
using CampusLost.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CampusLost.ViewModels;

public partial class FavoritesViewModel : ObservableObject
{
    readonly DataService data;
    public FavoritesViewModel(DataService data) => this.data = data;

    [ObservableProperty] private List<Item> items = new();
    [ObservableProperty] private string subtitle = "";

    public void Refresh()
    {
        Items = data.Favorites();
        Subtitle = Items.Count == 0
            ? "Gardez les annonces utiles à portée de main. Vous n'avez encore aucun favori."
            : $"Gardez les annonces utiles à portée de main. Vous avez enregistré {Items.Count} objet{(Items.Count > 1 ? "s" : "")}.";
    }

    [RelayCommand] Task SeeAll() => Shell.Current.GoToAsync("//main/objects");
}
