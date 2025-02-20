namespace Balatro_Instance_Manager.Helpers;
using System.Globalization;

public static class LocalizationManager
{
    public static CultureInfo CurrentCulture { get; private set; } = CultureInfo.CurrentCulture;
    public static Dictionary<string, string> LocalizedStrings { get; private set; } = new Dictionary<string, string>();

    public static void LoadLocalizations()
    {
        LocalizedStrings = new Dictionary<string, string>
        {
            ["DataInaccessible"] = Strings.DataInaccessible,
            ["SteamInaccessible"] = Strings.SteamInaccessible,
            ["NoModsDirectory"] = Strings.NoModsDirectory,
            ["AlreadyRunning"] = Strings.AlreadyRunning,
            ["BalatroNotFound"] = Strings.BalatroNotFound,
            ["ModsCleared"] = Strings.ModsCleared,
            ["MultiInstanceUnstable"] = Strings.MultiInstanceUnstable,
            
            // Window headers
            ["InfoHeader"] = Strings.InformationHeader,
            ["ErrorHeader"] = Strings.ErrorHeader,
            ["WarningHeader"] = Strings.WarningHeader
        };
    }
    
    // Implement eventually
    public static void SetCulture(string cultureName)
    {
        CurrentCulture = new CultureInfo(cultureName);
        Thread.CurrentThread.CurrentUICulture = CurrentCulture;
        Thread.CurrentThread.CurrentCulture = CurrentCulture;

        LoadLocalizations();
    }
}