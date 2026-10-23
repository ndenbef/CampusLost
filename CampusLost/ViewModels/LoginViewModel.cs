using CampusLost.Helpers;
using CampusLost.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CampusLost.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    readonly DataService data;
    public LoginViewModel(DataService data) => this.data = data;

    [ObservableProperty] private string email = "";
    [ObservableProperty] private string password = "";

    [ObservableProperty, NotifyPropertyChangedFor(nameof(PasswordGlyph))]
    private bool hidePassword = true;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(HasError))]
    private string error = "";

    public bool HasError => !string.IsNullOrEmpty(Error);
    public string PasswordGlyph => HidePassword ? "👁" : "🙈";

    [RelayCommand] void TogglePassword() => HidePassword = !HidePassword;

    [RelayCommand]
    async Task SignIn()
    {
        if (!Validation.IsEmail(Email)) { Error = "Saisissez une adresse e-mail valide."; return; }
        if (string.IsNullOrEmpty(Password)) { Error = "Saisissez votre mot de passe."; return; }
        Error = "";
        data.SignIn(Email);
        Password = "";
        await Shell.Current.GoToAsync("//main/home");
    }

    [RelayCommand]
    Task Forgot() => Shell.Current.DisplayAlert("Mot de passe oublié",
        "Un lien de réinitialisation sera envoyé à votre adresse universitaire.", "OK");

    [RelayCommand] Task CreateAccount() => Shell.Current.GoToAsync("register");
}
