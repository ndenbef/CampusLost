namespace CampusLost.Controls;

/// <summary>Vignette carrée d'un objet (photo ou emoji de la catégorie). BindingContext = Item.</summary>
public partial class Thumb : ContentView
{
    public static readonly BindableProperty SizeProperty =
        BindableProperty.Create(nameof(Size), typeof(double), typeof(Thumb), 84.0,
            propertyChanged: (b, _, _) => ((Thumb)b).Apply());

    public double Size { get => (double)GetValue(SizeProperty); set => SetValue(SizeProperty, value); }

    public Thumb()
    {
        InitializeComponent();
        HorizontalOptions = LayoutOptions.Start;
        VerticalOptions = LayoutOptions.Start;
        Apply();
    }

    void Apply()
    {
        WidthRequest = HeightRequest = Size;
        if (Emo != null) Emo.FontSize = Size * 0.45;
    }
}
