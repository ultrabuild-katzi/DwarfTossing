using DwarfTossing.Character.Player;
using Godot;
using static DwarfTossing.Character.Player.GameSettingsManager;

namespace DwarfTossing.Screen.Options;

public partial class Options : Control
{
    [Export] public required Slider MouseSensitivitySlider;
    [Export] public required Slider MasterVolumeSlider;
    [Export] public required Slider BrightnessSlider;
    [Export] public required OptionButton WindowModeButton;
    [Export] public required Button CancelButton;

    public void OnCancelButtonPressed()
    {
        GetParent<CanvasLayer>().Visible = false;
    }

    public void OnAcceptButtonPressed()
    {
        GameSettingsManager.GameSettings = new GameSettings
        {
            MouseSensitivity = (float)MouseSensitivitySlider.GetValue(),
            MasterVolume = (float)MasterVolumeSlider.GetValue(),
            Brightness = (float)BrightnessSlider.GetValue(),
            WindowMode = (DisplayServer.WindowMode) WindowModeButton.Selected,
        };

        SaveSettings();
    }

    public void OnDoneButtonPressed()
    {
        OnAcceptButtonPressed();
        OnCancelButtonPressed();
    }

    public void OnVisibilityChange()
    {
        if(!GetParent<CanvasLayer>().Visible)
            return;
        
        MouseSensitivitySlider.SetValue(GameSettingsManager.GameSettings.MouseSensitivity);
        MasterVolumeSlider.SetValue(GameSettingsManager.GameSettings.MasterVolume);
        BrightnessSlider.SetValue(GameSettingsManager.GameSettings.Brightness);
        WindowModeButton.Selected = (int) GameSettingsManager.GameSettings.WindowMode;
        CancelButton.GrabFocus();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (!GetParent<CanvasLayer>().Visible || !@event.IsActionPressed("ui_cancel"))
            return;
        OnCancelButtonPressed();
        GetViewport().SetInputAsHandled();
    }
}
