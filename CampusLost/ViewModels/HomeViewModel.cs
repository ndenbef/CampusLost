using CampusLost.Models;
using CampusLost.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CampusLost.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    readonly DataService data;
    public HomeViewModel(DataService data) => this.data = data;

    [ObservableProperty] private string search = "";
    [ObservableProperty] private Item? firstAd;
    [ObservableProperty] private bool hasAd;

    public void Refresh()
    {
        FirstAd = data.Mine(false).FirstOrDefault();
        HasAd = FirstAd != null;
    }

    [RelayCommand] Task ReportLost() => Shell.Current.GoToAsync("report?mode=lost");
    [RelayCommand] Task ReportFound() => Shell.Current.GoToAsync("report?mode=found");
    [RelayCommand] Task SeeAll() => Shell.Current.GoToAsync("myads");

    [RelayCommand]
    async Task DoSearch()
    {
        await Shell.Current.GoToAsync($"//main/objects?q={Uri.EscapeDataString(Search ?? "")}");
        Search = "";
    }

    [RelayCommand] Task Notifications() => Shell.Current.DisplayAlert("Notifications", "Aucune nouvelle notification.", "OK");
}
