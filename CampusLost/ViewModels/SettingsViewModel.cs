using CampusLost.Helpers;
using CampusLost.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CampusLost.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    readonly DataService data;
    public SettingsViewModel(DataService data) => this.data = data;

    public bool MessagesNotif { get => Preferences.Get("n_msg", true); set { Preferences.Set("n_msg", value); OnPropertyChanged(); } }
    public bool SimilarNotif { get => Preferences.Get("n_sim", true); set { Preferences.Set("n_sim", value); OnPropertyChanged(); } }
    public bool NewsNotif { get => Preferences.Get("n_news", false); set { Preferences.Set("n_news", value); OnPropertyChanged(); } }

    [ObservableProperty] private string campus = "";

    public void Refresh() => Campus = data.CurrentUser?.Campus ?? "";

    [RelayCommand]
    async Task ChangePassword()
    {
        var pwd = await Shell.Current.DisplayPromptAsync("Changer mon mot de passe", "Nouveau mot de passe (8 caractères min., une lettre et un chiffre) :", "Valider", "Annuler");
        if (pwd == null) return;
        if (!Validation.IsStrongPassword(pwd))
        { await Shell.Current.DisplayAlert("Mot de passe invalide", "8 caractères minimum, une lettre et un chiffre.", "OK"); return; }
        // TODO: appeler votre API quand un backend existera.
        await Shell.Current.DisplayAlert("Mot de passe modifié", "Votre mot de passe a été mis à jour.", "OK");
    }

    [RelayCommand]
    async Task ChooseCampus()
    {
        var c = await Shell.Current.DisplayActionSheet("Mon campus", "Annuler", null, DataService.Campuses);
        if (c is null or "Annuler" || data.CurrentUser == null) return;
        data.CurrentUser.Campus = c;
        Refresh();
    }

    [RelayCommand]
    Task ChooseLanguage() => Shell.Current.DisplayAlert("Langue de l'application", "Seul le français est disponible pour le moment.", "OK");

    [RelayCommand]
    Task Privacy() => Shell.Current.DisplayAlert("Données personnelles",
        "CampusLost ne partage jamais votre adresse e-mail avec les autres membres. Vous pouvez supprimer vos annonces et votre compte à tout moment.", "OK");

    [RelayCommand]
    Task Terms() => Shell.Current.DisplayAlert("Conditions d'utilisation",
        "Service gratuit réservé aux membres du campus. Les remises d'objets se font dans un lieu public. Aucun paiement ni mot de passe ne doit être échangé.", "OK");

    [RelayCommand]
    async Task DeleteAccount()
    {
        if (!await Shell.Current.DisplayAlert("Supprimer mon compte", "Cette action supprimera votre compte et vos annonces. Continuer ?", "Supprimer", "Annuler")) return;
        data.DeleteAccount();
        await Shell.Current.GoToAsync("//login");
    }
}
