using System.Globalization;
using CampusLost.Models;

namespace CampusLost.Services;

/// <summary>Données en mémoire (aucun backend). Remplaçable plus tard par une API.</summary>
public class DataService
{
    public const string AllCategories = "Toutes catégories";

    public static readonly string[] Categories =
    {
        "Sacs & accessoires", "Clés", "Électronique", "Documents & cartes",
        "Vêtements", "Bouteilles & gourdes", "Livres & fournitures", "Autre"
    };
    public static readonly string[] Campuses = { "Campus I", "Campus II", "Campus III" };

    public static string EmojiFor(string category) => category switch
    {
        "Sacs & accessoires" => "🎒",
        "Clés" => "🔑",
        "Électronique" => "🎧",
        "Documents & cartes" => "🪪",
        "Vêtements" => "🧥",
        "Bouteilles & gourdes" => "🧴",
        "Livres & fournitures" => "📘",
        _ => "📦"
    };

    readonly List<Item> items = new();
    readonly User demo = new() { FirstName = "Thomas", LastName = "Martin", Email = "thomas.martin@univ.fr", Campus = "Campus centre" };
    int counter = 24;

    public User? CurrentUser { get; private set; }

    public DataService() => Seed();

    // ---------- Auth (simulée) ----------
    public void SignIn(string email)
    {
        email = email.Trim();
        CurrentUser = email.Equals(demo.Email, StringComparison.OrdinalIgnoreCase) ? demo : FromEmail(email);
    }
    public void Register(User u) => CurrentUser = u;
    public void SignOut() => CurrentUser = null;
    public void DeleteAccount()
    {
        items.RemoveAll(i => i.OwnerEmail == CurrentUser?.Email);
        CurrentUser = null;
    }

    static User FromEmail(string email)
    {
        var parts = email.Split('@')[0].Split('.', '_', '-');
        string Cap(string s) => s.Length == 0 ? s : char.ToUpper(s[0]) + s[1..];
        return new User { Email = email, FirstName = Cap(parts[0]), LastName = parts.Length > 1 ? Cap(parts[1]) : "" };
    }

    // ---------- Requêtes ----------
    public Item? Get(string id) => items.FirstOrDefault(i => i.Id == id);

    public List<Item> Mine(bool resolved) =>
        items.Where(i => i.OwnerEmail == CurrentUser?.Email && i.IsResolved == resolved)
             .OrderByDescending(i => i.Date).ThenByDescending(i => i.Time).ToList();

    public int MineCount => items.Count(i => i.OwnerEmail == CurrentUser?.Email);
    public int ReturnedCount => items.Count(i => i.OwnerEmail == CurrentUser?.Email && i.IsResolved);

    public List<Item> Favorites() =>
        items.Where(i => i.IsFavorite).OrderByDescending(i => i.Date).ThenByDescending(i => i.Time).ToList();

    public List<Item> Search(string? query, string filter, string category, bool newestFirst)
    {
        IEnumerable<Item> r = items.Where(i => !i.IsResolved);
        if (filter == "lost") r = r.Where(i => i.IsLost);
        if (filter == "found") r = r.Where(i => !i.IsLost);
        if (!string.IsNullOrEmpty(category) && category != AllCategories) r = r.Where(i => i.Category == category);
        if (!string.IsNullOrWhiteSpace(query))
            r = r.Where(i => Has(i.Name, query) || Has(i.Location, query) || Has(i.LocationDetail, query) ||
                             Has(i.Category, query) || Has(i.Description, query));
        return (newestFirst
            ? r.OrderByDescending(i => i.Date).ThenByDescending(i => i.Time)
            : r.OrderBy(i => i.Date).ThenBy(i => i.Time)).ToList();
    }

    static bool Has(string text, string q) =>
        Item.Fr.CompareInfo.IndexOf(text, q.Trim(), CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;

    // ---------- Commandes ----------
    public void Add(Item item)
    {
        item.Id = $"CL{++counter:000}";
        item.OwnerEmail = CurrentUser?.Email ?? "";
        item.OwnerName = CurrentUser?.FullName ?? "";
        item.OwnerRole = $"Étudiant • {CurrentUser?.Campus}";
        items.Add(item);
    }

    public void Resolve(Item item)
    {
        item.IsResolved = true;
        item.ResolvedNote = $"{(item.IsLost ? "Rendue" : "Rendu")} à son propriétaire le {DateTime.Today.ToString("d MMM", Item.Fr)}.";
    }

    // ---------- Données de démonstration ----------
    void Seed()
    {
        var thomas = demo;
        const string marie = "marie.laurent@univ.fr";

        items.Add(new Item
        {
            Id = "CL024", Name = "Clés de maison", Category = "Clés", IsLost = false, Emoji = "🔑",
            Description = "Trois clés sur un anneau, avec un porte-clés en tissu bleu. Retrouvées sur une table. L'objet est déposé à l'accueil de la bibliothèque.",
            Location = "Bibliothèque universitaire", LocationDetail = "Salle de lecture, rez-de-chaussée",
            Date = new DateTime(2026, 10, 6), Time = new TimeSpan(10, 30, 0), CurrentLocation = "À l'accueil de la bibliothèque",
            OwnerEmail = marie, OwnerName = "Marie Laurent", OwnerRole = "Étudiante • Campus centre", IsFavorite = true
        });
        items.Add(new Item
        {
            Id = "CL023", Name = "Sac à dos noir", Category = "Sacs & accessoires", IsLost = true, Emoji = "🎒",
            Description = "Sac à dos noir avec une poche zippée à l'avant. Il contient un cahier bleu et une trousse grise.",
            Location = "Bâtiment B", LocationDetail = "Amphi 3, près du dernier rang",
            Date = new DateTime(2026, 10, 5), Time = new TimeSpan(14, 0, 0),
            OwnerEmail = thomas.Email, OwnerName = thomas.FullName, OwnerRole = "Étudiant • Campus centre"
        });
        items.Add(new Item
        {
            Id = "CL022", Name = "Gourde isotherme", Category = "Bouteilles & gourdes", IsLost = false, Emoji = "🧴",
            Description = "Gourde isotherme turquoise, laissée sur une table de la cafétéria.",
            Location = "Cafétéria", LocationDetail = "Campus centre",
            Date = new DateTime(2026, 10, 5), Time = new TimeSpan(9, 0, 0), CurrentLocation = "Au comptoir de la cafétéria",
            OwnerEmail = thomas.Email, OwnerName = thomas.FullName, OwnerRole = "Étudiant • Campus centre", IsFavorite = true
        });
        items.Add(new Item
        {
            Id = "CL021", Name = "Casque Bluetooth", Category = "Électronique", IsLost = true, Emoji = "🎧",
            Description = "Casque Bluetooth noir, perdu au premier étage de la bibliothèque.",
            Location = "Bibliothèque", LocationDetail = "1er étage",
            Date = new DateTime(2026, 10, 4), Time = new TimeSpan(16, 0, 0),
            OwnerEmail = "lucas.bernard@univ.fr", OwnerName = "Lucas Bernard", OwnerRole = "Étudiant • Campus centre", IsFavorite = true
        });
        items.Add(new Item
        {
            Id = "CL019", Name = "Carte étudiante", Category = "Documents & cartes", IsLost = true, Emoji = "🪪",
            Description = "Carte étudiante.", Location = "Bâtiment A", LocationDetail = "Hall",
            Date = new DateTime(2026, 10, 2), Time = new TimeSpan(11, 0, 0),
            OwnerEmail = thomas.Email, OwnerName = thomas.FullName, OwnerRole = "Étudiant • Campus centre",
            IsResolved = true, ResolvedNote = "Rendue à son propriétaire le 3 oct."
        });
    }
}
