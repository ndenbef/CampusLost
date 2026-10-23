using CampusLost.ViewModels;

namespace CampusLost.Views;

public partial class ContactPage : ContentPage
{
    public ContactPage(ContactViewModel vm) { InitializeComponent(); BindingContext = vm; }
}
