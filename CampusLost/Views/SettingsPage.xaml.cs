using CampusLost.ViewModels;

namespace CampusLost.Views;

public partial class SettingsPage : ContentPage
{
    readonly SettingsViewModel vm;
    public SettingsPage(SettingsViewModel vm) { InitializeComponent(); BindingContext = this.vm = vm; }
    protected override void OnAppearing() { base.OnAppearing(); vm.Refresh(); }
}
