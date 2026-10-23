using CampusLost.Models;

namespace CampusLost.Controls;

/// <summary>Carte d'objet réutilisée (accueil, objets, favoris). BindingContext = Item.</summary>
public partial class ItemCard : ContentView
{
    public ItemCard() => InitializeComponent();

    async void OnCardTapped(object? sender, TappedEventArgs e)
    {
        if (BindingContext is Item item) await Shell.Current.GoToAsync($"details?id={item.Id}");
    }

    void OnHeartTapped(object? sender, TappedEventArgs e)
    {
        if (BindingContext is Item item) item.IsFavorite = !item.IsFavorite;
    }
}
