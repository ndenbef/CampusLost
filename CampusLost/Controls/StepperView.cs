using CampusLost.Helpers;
using Microsoft.Maui.Controls.Shapes;

namespace CampusLost.Controls;

/// <summary>Indicateur d'étapes 1-4 (Infos objet • Détails • Photo • Confirmation).</summary>
public class StepperView : ContentView
{
    public static readonly BindableProperty StepProperty =
        BindableProperty.Create(nameof(Step), typeof(int), typeof(StepperView), 1,
            propertyChanged: (b, _, _) => ((StepperView)b).Build());

    public int Step { get => (int)GetValue(StepProperty); set => SetValue(StepProperty, value); }

    static readonly string[] Names = { "Infos objet", "Détails", "Photo", "Confirmation" };

    public StepperView() => Build();

    void Build()
    {
        var grid = new Grid { ColumnSpacing = 8, RowSpacing = 8 };
        for (int i = 0; i < 4; i++) grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        for (int i = 0; i < 3; i++) grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

        for (int i = 0; i < 4; i++)
        {
            int n = i + 1;
            bool reached = Step >= n, done = Step > n, current = Step == n;

            grid.Add(new BoxView { HeightRequest = 4, CornerRadius = 2, Color = reached ? Theme.Green : Theme.LineStrong }, i, 0);

            var circle = new Border
            {
                WidthRequest = 34, HeightRequest = 34, HorizontalOptions = LayoutOptions.Center,
                StrokeShape = new RoundRectangle { CornerRadius = 17 }, StrokeThickness = 1,
                Stroke = reached ? Theme.Green : Theme.Line,
                BackgroundColor = reached ? Theme.Green : Colors.White,
                Content = new Label
                {
                    Text = done ? "✓" : n.ToString(), FontFamily = "OpenSansSemibold", FontSize = 14,
                    TextColor = reached ? Colors.White : Theme.Muted,
                    HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center
                }
            };
            grid.Add(circle, i, 1);

            grid.Add(new Label
            {
                Text = Names[i], FontSize = 12, HorizontalTextAlignment = TextAlignment.Center,
                FontFamily = current ? "OpenSansSemibold" : "OpenSansRegular",
                TextColor = current ? Theme.Green : Theme.Muted, LineBreakMode = LineBreakMode.NoWrap
            }, i, 2);
        }
        Content = grid;
    }
}
