using CampusLost.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CampusLost.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    readonly DataService data;
    public ProfileViewModel(DataService data) => this.data = data;

    [ObservableProperty] private string fullName = "";
    [ObservableProperty] private string email = "";
    [ObservableProperty] private string roleLine = "";
    [ObservableProperty] private int adsCount;
    [ObservableProperty] private int favCount;
    [ObservableProperty] private int returnedCount;

    public void Refresh()
    {
        var u = data.CurrentUser;
        FullName = u?.FullName ?? "";
        Email = u?.Email ?? "";
        RoleLine = $"Étudiant • {u?.Campus}";
        AdsCount = data.MineCount;
        FavCount = data.Favorites().Count;
        ReturnedCount = data.ReturnedCount;
    }

    [RelayCommand] Task Go(string route) => Shell.Current.GoToAsync(route);

    [RelayCommand]
    async Task EditProfile()
    {
        var r = await Shell.Current.DisplayPromptAsync("Modifier mon profil", "Prénom et nom :", "Enregistrer", "Annuler", initialValue: FullName);
        if (string.IsNullOrWhiteSpace(r) || data.CurrentUser == null) return;
        var parts = r.Trim().Split(' ', 2);
        data.CurrentUser.FirstName = parts[0];
        data.CurrentUser.LastName = parts.Length > 1 ? parts[1] : "";
        Refresh();
    }

    [RelayCommand]
    async Task SignOut()
    {
        if (!await Shell.Current.DisplayAlert("Déconnexion", "Voulez-vous vous déconnecter ?", "Se déconnecter", "Annuler")) return;
        data.SignOut();
        await Shell.Current.GoToAsync("//login");
    }
}
