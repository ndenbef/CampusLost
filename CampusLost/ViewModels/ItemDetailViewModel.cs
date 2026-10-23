using CampusLost.Models;
using CampusLost.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CampusLost.ViewModels;

[QueryProperty(nameof(Id), "id")]
public partial class ItemDetailViewModel : ObservableObject
{
    readonly DataService data;
    public ItemDetailViewModel(DataService data) => this.data = data;

    [ObservableProperty] private string id = "";
    [ObservableProperty] private Item? ad;
    [ObservableProperty] private bool canContact;

    partial void OnIdChanged(string value)
    {
        Ad = data.Get(value);
        CanContact = Ad != null && Ad.OwnerEmail != data.CurrentUser?.Email;
    }

    [RelayCommand] void ToggleFavorite() { if (Ad != null) Ad.IsFavorite = !Ad.IsFavorite; }
    [RelayCommand] Task Contact() => Shell.Current.GoToAsync($"contact?id={Id}");

    [RelayCommand]
    async Task ReportAbuse()
    {
        if (await Shell.Current.DisplayAlert("Signaler une annonce", "Voulez-vous signaler cette annonce comme inappropriée ?", "Signaler", "Annuler"))
            await Shell.Current.DisplayAlert("Merci", "Votre signalement a été transmis à l'équipe de modération.", "OK");
    }
}
