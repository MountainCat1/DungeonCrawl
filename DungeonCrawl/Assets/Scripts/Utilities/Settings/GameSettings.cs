using Utilities;

public class GameSettings : ISaveable<GameSettings>
{
    public static GameSettings Instance
    {
        get
        {
            var settings = SaveLoadManager.Load<GameSettings>();
            
            return settings;
        }
    }

    public bool FallbackToDefaultLanguage { get; set; }
    public string Language { get; set; } = "En";
    public bool UsePixelArtFont { get; set; }
    

    public static void Update(GameSettings instance) => SaveLoadManager.Update(instance);
    public static void Save() => SaveLoadManager.Save(Instance);

    public string GetFileName() => "settings.json";

    public GameSettings CreateDefault()
    {
        return new GameSettings
        {
        };
    }
}
