using CampusLost.Views;

namespace CampusLost;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("register", typeof(RegisterPage));
        Routing.RegisterRoute("details", typeof(ItemDetailPage));
        Routing.RegisterRoute("contact", typeof(ContactPage));
        Routing.RegisterRoute("report", typeof(ReportPage));
        Routing.RegisterRoute("published", typeof(PublishedPage));
        Routing.RegisterRoute("myads", typeof(MyAdsPage));
        Routing.RegisterRoute("settings", typeof(SettingsPage));
        Routing.RegisterRoute("help", typeof(HelpPage));
        Routing.RegisterRoute("about", typeof(AboutPage));
    }
}
