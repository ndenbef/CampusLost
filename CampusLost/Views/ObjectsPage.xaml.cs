using CampusLost.ViewModels;

namespace CampusLost.Views;

public partial class ObjectsPage : ContentPage
{
    readonly ObjectsViewModel vm;
    public ObjectsPage(ObjectsViewModel vm) { InitializeComponent(); BindingContext = this.vm = vm; }
    protected override void OnAppearing() { base.OnAppearing(); vm.Refresh(); }
}
