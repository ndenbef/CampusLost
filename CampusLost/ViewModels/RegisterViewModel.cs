using CampusLost.Helpers;
using CampusLost.Models;
using CampusLost.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CampusLost.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    readonly DataService data;
    public RegisterViewModel(DataService data) => this.data = data;

    public string[] Campuses => DataService.Campuses;

    [ObservableProperty] private string firstName = "";
    [ObservableProperty] private string lastName = "";
    [ObservableProperty] private string email = "";
    [ObservableProperty] private string campus = DataService.Campuses[0];
    [ObservableProperty] private string password = "";
    [ObservableProperty] private string confirm = "";
    [ObservableProperty] private bool accepted;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(PasswordGlyph))]
    private bool hidePassword = true;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(HasError))]
    private string error = "";

    public bool HasError => !string.IsNullOrEmpty(Error);
    public string PasswordGlyph => HidePassword ? "👁" : "🙈";

    [RelayCommand] void TogglePassword() => HidePassword = !HidePassword;

    [RelayCommand]
    async Task CreateAccount()
    {
        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName)) { Error = "Renseignez votre prénom et votre nom."; return; }
        if (!Validation.IsEmail(Email)) { Error = "Saisissez une adresse e-mail universitaire valide."; return; }
        if (!Validation.IsStrongPassword(Password)) { Error = "Mot de passe : 8 caractères minimum, une lettre et un chiffre."; return; }
        if (Password != Confirm) { Error = "Les mots de passe ne correspondent pas."; return; }
        if (!Accepted) { Error = "Acceptez les conditions d'utilisation pour continuer."; return; }

        data.Register(new User { FirstName = FirstName.Trim(), LastName = LastName.Trim(), Email = Email.Trim(), Campus = Campus });
        await Shell.Current.GoToAsync("//main/home");
    }

    [RelayCommand] Task BackToLogin() => Shell.Current.GoToAsync("..");
}
