using CampusLost.Models;
using CampusLost.ViewModels;

namespace CampusLost.Views;

public partial class MyAdsPage : ContentPage
{
    readonly MyAdsViewModel vm;
    public MyAdsPage(MyAdsViewModel vm) { InitializeComponent(); BindingContext = this.vm = vm; }
    protected override void OnAppearing() { base.OnAppearing(); vm.Refresh(); }

    async void OnEditTapped(object? sender, TappedEventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is Item item) await vm.Edit(item);
    }

    async void OnResolveTapped(object? sender, TappedEventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is Item item) await vm.MarkResolved(item);
    }
}
