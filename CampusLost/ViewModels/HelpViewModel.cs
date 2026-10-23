using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CampusLost.ViewModels;

public partial class FaqEntry : ObservableObject
{
    public string Question { get; init; } = "";
    public string Answer { get; init; } = "";

    [ObservableProperty, NotifyPropertyChangedFor(nameof(Chevron))]
    private bool isOpen;

    public string Chevron => IsOpen ? "⌃" : "›";
}

public partial class HelpViewModel : ObservableObject
{
    readonly List<FaqEntry> all = new()
    {
        new() { Question = "Comment récupérer mon objet ?", IsOpen = true,
            Answer = "Ouvrez l'annonce et touchez « Contacter l'annonceur ». Décrivez un détail qui vous permet d'identifier l'objet, puis convenez d'une remise dans un lieu public sur le campus." },
        new() { Question = "Comment publier un signalement ?",
            Answer = "Depuis l'accueil, touchez « Objet perdu » ou « Objet trouvé », puis suivez les 4 étapes : infos, lieu et date, photo, confirmation." },
        new() { Question = "Puis-je modifier mon annonce ?",
            Answer = "Oui. Dans Profil > Mes annonces, touchez « Modifier l'annonce » ou « Marquer comme résolu »." },
        new() { Question = "Comment signaler un abus ?",
            Answer = "Ouvrez l'annonce concernée et touchez « Signaler une annonce inappropriée » en bas de page." },
        new() { Question = "Mes coordonnées sont-elles visibles ?",
            Answer = "Non. Votre adresse e-mail reste privée : les messages passent par l'application et les réponses vous sont envoyées par e-mail." },
    };

    [ObservableProperty] private string search = "";
    [ObservableProperty] private List<FaqEntry> entries = new();

    public HelpViewModel() => Entries = all;

    partial void OnSearchChanged(string value) =>
        Entries = string.IsNullOrWhiteSpace(value)
            ? all
            : all.Where(e => e.Question.Contains(value, StringComparison.CurrentCultureIgnoreCase) ||
                             e.Answer.Contains(value, StringComparison.CurrentCultureIgnoreCase)).ToList();

    public void Toggle(FaqEntry entry)
    {
        var open = !entry.IsOpen;
        foreach (var e in all) e.IsOpen = false;
        entry.IsOpen = open;
    }

    [RelayCommand]
    async Task ContactSupport()
    {
        try { await Launcher.Default.OpenAsync("mailto:assistance@campuslost.app?subject=Assistance%20CampusLost"); }
        catch { await Shell.Current.DisplayAlert("Assistance", "Écrivez-nous à assistance@campuslost.app (réponse sous 48 h, du lundi au vendredi).", "OK"); }
    }
}
