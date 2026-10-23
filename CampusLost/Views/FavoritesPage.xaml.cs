using CampusLost.ViewModels;

namespace CampusLost.Views;

public partial class FavoritesPage : ContentPage
{
    readonly FavoritesViewModel vm;
    public FavoritesPage(FavoritesViewModel vm) { InitializeComponent(); BindingContext = this.vm = vm; }
    protected override void OnAppearing() { base.OnAppearing(); vm.Refresh(); }
}
