# CampusLost – .NET MAUI (.NET 8)

## Visual Studio 2022
1. VS 2022 17.8+ avec la charge de travail **« Développement d'applications mobiles multiplateformes avec .NET »** (.NET MAUI) et le SDK .NET 8.
2. Double-cliquer sur **CampusLost.sln**. La restauration NuGet démarre toute seule.
3. Barre d'outils : choisir la cible
   - **Windows Machine** : lance l'app sur le PC, sans émulateur (le plus simple pour tester).
   - **Android Emulator** : créer un émulateur via Outils > Android > Android Device Manager.
4. F5.

## VS Code
Ouvrir le dossier, installer « C# Dev Kit » + « .NET MAUI », puis :
`dotnet build CampusLost/CampusLost.csproj -t:Run -f net8.0-android`
(ou `-f net8.0-windows10.0.19041.0` sous Windows).

## Compte de démonstration
`thomas.martin@univ.fr` + n'importe quel mot de passe → charge les annonces de la maquette.
Tout autre e-mail valide ouvre une session vide.

## Points à brancher plus tard
- `Services/DataService.cs` : remplacer par une API.
- `Controls/CampusBanner.xaml` : mettre l'illustration du campus.
