using Godot;
using System;

namespace DwarfTossing.Character.Player;

public record struct GameSettings(
    float MouseSensitivity,
    float MasterVolume,
    float Brightness,
    DisplayServer.WindowMode WindowMode
)
{
    public static readonly GameSettings Defaults = new(1.0f, 1.0f, 1.0f, DisplayServer.WindowMode.ExclusiveFullscreen);
    
    
    public readonly void Write(ConfigFile configFile)
    {
        configFile.SetValue("input", "MouseSensitivity", MouseSensitivity);
        configFile.SetValue("audio", "MasterVolume", MasterVolume);
        configFile.SetValue("video", "Brightness", Brightness);
        configFile.SetValue("video", "WindowMode", WindowMode.ToString());
    }

    public void Read(ConfigFile configFile)
    {
        MouseSensitivity = (float) configFile.GetValue("input", "MouseSensitivity", Defaults.MouseSensitivity);
        MasterVolume = (float) configFile.GetValue("audio", "MasterVolume", Defaults.MasterVolume);
        Brightness = (float) configFile.GetValue("video", "Brightness", Defaults.Brightness);
        WindowMode = Enum.Parse<DisplayServer.WindowMode>(configFile.GetValue("video", "WindowMode", Defaults.WindowMode.ToString()).AsString(), true);
    }
}
