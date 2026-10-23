using CampusLost.ViewModels;

namespace CampusLost.Views;

public partial class PublishedPage : ContentPage
{
    public PublishedPage(PublishedViewModel vm) { InitializeComponent(); BindingContext = vm; }
}
