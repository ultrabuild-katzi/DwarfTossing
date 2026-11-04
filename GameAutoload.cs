using DwarfTossing.Character.Player;
using Godot;

public partial class GameAutoload : Node
{
    public override void _Ready()
    {
        GameSettingsManager.Initialize();
    }
}
