namespace CampusLost.Controls;

public partial class AppHeader : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(AppHeader), string.Empty);
    public static readonly BindableProperty ShowBackProperty =
        BindableProperty.Create(nameof(ShowBack), typeof(bool), typeof(AppHeader), true);
    public static readonly BindableProperty ShowMenuProperty =
        BindableProperty.Create(nameof(ShowMenu), typeof(bool), typeof(AppHeader), true);

    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
    public bool ShowBack { get => (bool)GetValue(ShowBackProperty); set => SetValue(ShowBackProperty, value); }
    public bool ShowMenu { get => (bool)GetValue(ShowMenuProperty); set => SetValue(ShowMenuProperty, value); }

    public AppHeader() => InitializeComponent();

    async void OnBack(object? sender, TappedEventArgs e)
    {
        var shell = Shell.Current;
        if (shell.Navigation.NavigationStack.Count > 1) await shell.GoToAsync("..");
        else await shell.GoToAsync("//main/home");
    }

    async void OnMenu(object? sender, TappedEventArgs e)
    {
        var choice = await Shell.Current.DisplayActionSheet("Menu", "Annuler", null, "Aide & FAQ", "Paramètres", "À propos");
        switch (choice)
        {
            case "Aide & FAQ": await Shell.Current.GoToAsync("help"); break;
            case "Paramètres": await Shell.Current.GoToAsync("settings"); break;
            case "À propos": await Shell.Current.GoToAsync("about"); break;
        }
    }
}
