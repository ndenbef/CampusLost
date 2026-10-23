using CampusLost.ViewModels;

namespace CampusLost.Views;

public partial class HelpPage : ContentPage
{
    readonly HelpViewModel vm;
    public HelpPage(HelpViewModel vm) { InitializeComponent(); BindingContext = this.vm = vm; }

    void OnEntryTapped(object? sender, TappedEventArgs e)
    {
        if ((sender as BindableObject)?.BindingContext is FaqEntry entry) vm.Toggle(entry);
    }
}
