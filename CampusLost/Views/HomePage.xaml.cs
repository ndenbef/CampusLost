using CampusLost.ViewModels;

namespace CampusLost.Views;

public partial class HomePage : ContentPage
{
    readonly HomeViewModel vm;
    public HomePage(HomeViewModel vm) { InitializeComponent(); BindingContext = this.vm = vm; }
    protected override void OnAppearing() { base.OnAppearing(); vm.Refresh(); }
}
