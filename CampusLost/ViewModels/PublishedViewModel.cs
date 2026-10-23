using CampusLost.Models;
using CampusLost.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CampusLost.ViewModels;

[QueryProperty(nameof(Id), "id")]
public partial class PublishedViewModel : ObservableObject
{
    readonly DataService data;
    public PublishedViewModel(DataService data) => this.data = data;

    [ObservableProperty] private string id = "";
    [ObservableProperty] private Item? ad;
    [ObservableProperty] private string subtitle = "";

    partial void OnIdChanged(string value)
    {
        Ad = data.Get(value);
        Subtitle = Ad is { IsLost: false }
            ? "Votre annonce est maintenant visible sur le campus. Son propriétaire pourra vous contacter."
            : "Votre annonce est maintenant visible sur le campus. Nous espérons que vous retrouverez vite votre objet.";
    }

    [RelayCommand]
    async Task ViewAd()
    {
        await Shell.Current.GoToAsync("//main/home");
        await Shell.Current.GoToAsync($"details?id={Id}");
    }

    [RelayCommand] Task Home() => Shell.Current.GoToAsync("//main/home");
}
