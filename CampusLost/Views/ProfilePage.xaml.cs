using CampusLost.ViewModels;

namespace CampusLost.Views;

public partial class ProfilePage : ContentPage
{
    readonly ProfileViewModel vm;
    public ProfilePage(ProfileViewModel vm) { InitializeComponent(); BindingContext = this.vm = vm; }
    protected override void OnAppearing() { base.OnAppearing(); vm.Refresh(); }
}
