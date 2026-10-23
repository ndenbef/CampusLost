using CampusLost.Helpers;
using CampusLost.Models;
using CampusLost.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CampusLost.ViewModels;

[QueryProperty(nameof(Mode), "mode")]
[QueryProperty(nameof(EditId), "edit")]
public partial class ReportViewModel : ObservableObject
{
    readonly DataService data;
    Item? editing;

    public ReportViewModel(DataService data) => this.data = data;

    public string[] Categories => DataService.Categories;

    // ---- Mode ----
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsFound), nameof(Title), nameof(PlaceLabel), nameof(DateLabel),
        nameof(Step2Hint), nameof(StatusLabel), nameof(PillBg), nameof(PillFg))]
    private string mode = "lost";

    [ObservableProperty] private string editId = "";

    [ObservableProperty, NotifyPropertyChangedFor(nameof(Title), nameof(PublishText))]
    private bool isEditing;

    public bool IsFound => Mode == "found";
    public string Title => IsEditing ? "Modifier l'annonce" : IsFound ? "Signaler un objet trouvé" : "Signaler un objet perdu";
    public string PlaceLabel => IsFound ? "Lieu de découverte *" : "Lieu de perte *";
    public string DateLabel => IsFound ? "Date de découverte *" : "Date de perte *";
    public string Step2Hint => IsFound
        ? "Indiquez un lieu de remise sûr et accessible sur le campus."
        : "Plus le lieu est précis, plus vous avez de chances de retrouver votre objet.";
    public string StatusLabel => IsFound ? "Objet trouvé" : "Objet perdu";
    public Color PillBg => IsFound ? Theme.GreenSoft : Theme.RedSoft;
    public Color PillFg => IsFound ? Color.FromArgb("#00905F") : Theme.Red;
    public string PublishText => IsEditing ? "Enregistrer les modifications" : "Publier le signalement";

    // ---- Étapes ----
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsStep1), nameof(IsStep2), nameof(IsStep3), nameof(IsStep4),
        nameof(CanGoBack), nameof(IsNotLast), nameof(FooterText))]
    private int step = 1;

    public bool IsStep1 => Step == 1;
    public bool IsStep2 => Step == 2;
    public bool IsStep3 => Step == 3;
    public bool IsStep4 => Step == 4;
    public bool CanGoBack => Step > 1;
    public bool IsNotLast => Step < 4;
    public string FooterText => Step == 4
        ? "Publication gratuite • Visible sur votre campus"
        : "* Champs obligatoires • Votre progression est enregistrée";

    // ---- Champs ----
    [ObservableProperty] private string name = "";
    [ObservableProperty] private string category = "";
    [ObservableProperty] private string description = "";
    [ObservableProperty] private string place = "";
    [ObservableProperty] private string placeDetail = "";
    [ObservableProperty] private DateTime date = DateTime.Today;
    [ObservableProperty] private TimeSpan time = new(DateTime.Now.Hour, DateTime.Now.Minute, 0);
    [ObservableProperty] private string currentLocation = "";
    [ObservableProperty] private bool consent = true;
    [ObservableProperty] private string photoInfo = "";

    [ObservableProperty, NotifyPropertyChangedFor(nameof(HasPhoto), nameof(NoPhoto))]
    private string? photoPath;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(HasError))]
    private string error = "";

    public bool HasPhoto => !string.IsNullOrEmpty(PhotoPath);
    public bool NoPhoto => !HasPhoto;
    public bool HasError => !string.IsNullOrEmpty(Error);

    // ---- Récapitulatif (étape 4) ----
    public string ConfirmWhere => Item.BuildWhere(Place, PlaceDetail);
    public string ConfirmWhen => $"{Date.ToString("d MMMM yyyy", Item.Fr)}, vers {Time:hh\\:mm}";
    public string ConfirmEmoji => DataService.EmojiFor(Category);

    void RefreshSummary()
    {
        OnPropertyChanged(nameof(ConfirmWhere));
        OnPropertyChanged(nameof(ConfirmWhen));
        OnPropertyChanged(nameof(ConfirmEmoji));
    }

    // ---- Mode édition ----
    partial void OnEditIdChanged(string value)
    {
        editing = data.Get(value);
        if (editing == null) return;
        IsEditing = true;
        Mode = editing.IsLost ? "lost" : "found";
        Name = editing.Name; Category = editing.Category; Description = editing.Description;
        Place = editing.Location; PlaceDetail = editing.LocationDetail;
        Date = editing.Date; Time = editing.Time; CurrentLocation = editing.CurrentLocation;
        PhotoPath = editing.PhotoPath;
        PhotoInfo = HasPhoto ? "Photo actuelle" : "";
    }

    // ---- Navigation entre étapes ----
    [RelayCommand]
    void Next()
    {
        if (Step == 1 && (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Category) || string.IsNullOrWhiteSpace(Description)))
        { Error = "Renseignez le nom, la catégorie et la description."; return; }
        if (Step == 2 && string.IsNullOrWhiteSpace(Place))
        { Error = "Indiquez le lieu."; return; }

        Error = "";
        Step++;
        if (Step == 4) RefreshSummary();
    }

    [RelayCommand]
    void Previous()
    {
        if (Step > 1) { Step--; Error = ""; }
    }

    // ---- Photo ----
    [RelayCommand] Task PickPhoto() => SetPhoto(() => MediaPicker.Default.PickPhotoAsync());

    [RelayCommand]
    Task TakePhoto()
    {
        if (!MediaPicker.Default.IsCaptureSupported) { Error = "L'appareil photo n'est pas disponible."; return Task.CompletedTask; }
        return SetPhoto(() => MediaPicker.Default.CapturePhotoAsync());
    }

    [RelayCommand] void RemovePhoto() { PhotoPath = null; PhotoInfo = ""; }

    async Task SetPhoto(Func<Task<FileResult?>> pick)
    {
        try
        {
            var file = await pick();
            if (file == null) return;

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (ext is not (".jpg" or ".jpeg" or ".png")) { Error = "Format non pris en charge : JPG ou PNG uniquement."; return; }

            var dest = Path.Combine(FileSystem.CacheDirectory, $"{Guid.NewGuid():N}{ext}");
            using (var src = await file.OpenReadAsync())
            using (var dst = File.Create(dest))
                await src.CopyToAsync(dst);

            var size = new FileInfo(dest).Length;
            if (size > 5 * 1024 * 1024) { File.Delete(dest); Error = "La photo dépasse 5 Mo."; return; }

            PhotoPath = dest;
            PhotoInfo = $"{file.FileName} • {(size / 1048576.0).ToString("0.0", Item.Fr)} Mo";
            Error = "";
        }
        catch (Exception)
        {
            Error = "Impossible d'ajouter la photo (autorisation refusée ?).";
        }
    }

    // ---- Publication ----
    [RelayCommand]
    async Task Publish()
    {
        if (!Consent) { Error = "Acceptez d'être contacté par les membres du campus pour publier."; return; }
        Error = "";

        var item = editing ?? new Item();
        item.IsLost = !IsFound;
        item.Name = Name.Trim();
        item.Category = Category;
        item.Description = Description.Trim();
        item.Location = Place.Trim();
        item.LocationDetail = PlaceDetail?.Trim() ?? "";
        item.Date = Date;
        item.Time = Time;
        item.CurrentLocation = CurrentLocation?.Trim() ?? "";
        item.PhotoPath = PhotoPath;
        item.Emoji = DataService.EmojiFor(Category);

        if (editing == null)
        {
            data.Add(item);
            await Shell.Current.GoToAsync($"../published?id={item.Id}");
        }
        else
        {
            await Shell.Current.DisplayAlert("Annonce modifiée", "Vos modifications ont été enregistrées.", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}
