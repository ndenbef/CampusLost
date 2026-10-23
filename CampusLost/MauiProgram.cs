using Microsoft.Extensions.DependencyInjection;
using CampusLost.Services;
using CampusLost.ViewModels;
using CampusLost.Views;

namespace CampusLost;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });

        builder.Services.AddSingleton<DataService>();

        builder.Services.Page<LoginPage, LoginViewModel>();
        builder.Services.Page<RegisterPage, RegisterViewModel>();
        builder.Services.Page<HomePage, HomeViewModel>();
        builder.Services.Page<ObjectsPage, ObjectsViewModel>();
        builder.Services.Page<ItemDetailPage, ItemDetailViewModel>();
        builder.Services.Page<ContactPage, ContactViewModel>();
        builder.Services.Page<ReportPage, ReportViewModel>();
        builder.Services.Page<PublishedPage, PublishedViewModel>();
        builder.Services.Page<MyAdsPage, MyAdsViewModel>();
        builder.Services.Page<FavoritesPage, FavoritesViewModel>();
        builder.Services.Page<ProfilePage, ProfileViewModel>();
        builder.Services.Page<SettingsPage, SettingsViewModel>();
        builder.Services.Page<HelpPage, HelpViewModel>();
        builder.Services.AddTransient<AboutPage>();

        return builder.Build();
    }

    static void Page<TPage, TVm>(this IServiceCollection s) where TPage : class where TVm : class
    {
        s.AddTransient<TVm>();
        s.AddTransient<TPage>();
    }
}
