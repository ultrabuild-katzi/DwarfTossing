using Godot;

namespace DwarfTossing.Character.Player;

public static class GameSettingsManager
{
    public static GameSettings GameSettings = GameSettings.Defaults;
    private const string SettingsPath = "user://settings.cfg";
    private static DisplayServer.WindowMode _currentWindowMode = DisplayServer.WindowMode.Windowed; 

    static GameSettingsManager()
    {
        LoadSettings();
    }

    public static void UpdateSettings()
    {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), GameSettings.MasterVolume);
        if(_currentWindowMode != GameSettings.WindowMode)
            DisplayServer.WindowSetMode(GameSettings.WindowMode);
    }
    
    public static void ApplySettingsOnResume(World world)
    {
        world.GetNode<WorldEnvironment>("WorldEnvironment").Environment.SetAdjustmentBrightness(GameSettings.Brightness);
    }

    private static void LoadSettings()
    {
        ConfigFile configFile = new();
        if (configFile.Load(SettingsPath) != Error.Ok)
            return;

        GameSettings.Read(configFile);
        UpdateSettings();
    }

    public static void SaveSettings()
    {
        ConfigFile configFile = new();
        GameSettings.Write(configFile);
        configFile.Save(SettingsPath);
        UpdateSettings();
    }

    public static void Initialize()
    {
        GD.Print("LOADED SETTINGS");
    }

}
