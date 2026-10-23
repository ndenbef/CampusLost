using CampusLost.Models;
using CampusLost.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CampusLost.ViewModels;

[QueryProperty(nameof(Id), "id")]
public partial class ContactViewModel : ObservableObject
{
    readonly DataService data;
    public ContactViewModel(DataService data) => this.data = data;

    [ObservableProperty] private string id = "";
    [ObservableProperty] private Item? ad;
    [ObservableProperty] private string subject = "";
    [ObservableProperty] private string message = "";
    [ObservableProperty] private string firstNameOwner = "";

    [ObservableProperty, NotifyPropertyChangedFor(nameof(HasError))]
    private string error = "";

    public bool HasError => !string.IsNullOrEmpty(Error);
    public string SenderLine => $"Votre nom : {data.CurrentUser?.FullName} • Votre e-mail reste privé.";
    public string ReplyLine => $"La réponse de {FirstNameOwner} vous sera envoyée par e-mail. Organisez la remise dans un lieu public.";

    partial void OnIdChanged(string value)
    {
        Ad = data.Get(value);
        if (Ad == null) return;
        FirstNameOwner = Ad.OwnerName.Split(' ')[0];
        Subject = $"À propos de : {Ad.Name.ToLower(Item.Fr)}";
        Message = $"Bonjour {FirstNameOwner}, ";
        OnPropertyChanged(nameof(ReplyLine));
        OnPropertyChanged(nameof(SenderLine));
    }

    [RelayCommand]
    async Task Send()
    {
        if (Message.Trim().Length < 15) { Error = "Décrivez un détail distinctif dans votre message."; return; }
        Error = "";
        await Shell.Current.DisplayAlert("Message envoyé", $"{FirstNameOwner} recevra votre message et vous répondra par e-mail.", "OK");
        await Shell.Current.GoToAsync("..");
    }
}
