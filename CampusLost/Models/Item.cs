using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CampusLost.Models;

public partial class Item : ObservableObject
{
    public static readonly CultureInfo Fr = new("fr-FR");

    public string Id { get; set; } = "";
    public string OwnerEmail { get; set; } = "";
    public string OwnerName { get; set; } = "";
    public string OwnerRole { get; set; } = "";

    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Description { get; set; } = "";
    public bool IsLost { get; set; }
    public string Location { get; set; } = "";
    public string LocationDetail { get; set; } = "";
    public string CurrentLocation { get; set; } = "";
    public DateTime Date { get; set; } = DateTime.Today;
    public TimeSpan Time { get; set; }
    public string? PhotoPath { get; set; }
    public string Emoji { get; set; } = "📦";
    public bool IsResolved { get; set; }
    public string ResolvedNote { get; set; } = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HeartGlyph), nameof(HeartColor))]
    private bool isFavorite;

    public string HeartGlyph => IsFavorite ? "♥" : "♡";
    public Color HeartColor => IsFavorite ? Helpers.Theme.Green : Helpers.Theme.Muted;
    public bool HasPhoto => !string.IsNullOrEmpty(PhotoPath);
    public bool NoPhoto => !HasPhoto;

    public string StatusText => IsLost ? "Perdu" : "Trouvé";
    public string BadgeText => IsLost ? "Objet perdu" : "Objet trouvé";
    public Color PillBg => IsLost ? Helpers.Theme.RedSoft : Helpers.Theme.GreenSoft;
    public Color PillFg => IsLost ? Helpers.Theme.Red : Color.FromArgb("#00905F");

    public string Where => BuildWhere(Location, LocationDetail);
    public string DateShort => Date.ToString("d MMM yyyy", Fr);
    public string DateLine => $"{(IsLost ? "Perdu" : "Trouvé")} le {Date.ToString("d MMMM yyyy", Fr)}";
    public string TimeLine => $"Vers {Time:hh\\:mm}";
    public string CategoryAndId => $"{Category} • Annonce #{Id}";
    public string ResolvedBadge => "Résolue • Objet rendu";

    public string OwnerInitials =>
        string.Concat(OwnerName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(2).Select(p => char.ToUpper(p[0])));

    public static string BuildWhere(string location, string detail)
    {
        var place = location.Replace(" universitaire", "").Trim();
        var d = detail.Split(',')[0].Trim();
        return d.Length == 0 ? place : $"{place} • {d}";
    }
}
