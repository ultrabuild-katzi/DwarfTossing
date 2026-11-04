using DwarfTossing.Character.Player;
using Godot;
using System;

public partial class World : Node3D
{
	public override void _Ready()
	{
        GameSettingsManager.ApplySettingsOnResume(this);
	}

    public override void _Notification(int what)
    {
        if (what == NotificationUnpaused)
        {
            GameSettingsManager.ApplySettingsOnResume(this);
        }
    }

    public override void _Process(double delta)
	{
	}
}
