using DwarfTossing.Character.Player;
using Godot;
using System;
using System.Linq;

public partial class ExitWatcher : Node3D
{
	[Export] private Node3D _caveDecorations;
	[Signal] public delegate void ExitEventHandler();

	public override void _Process(double delta)
	{
		int count = GetNode("../cave_decorations").GetChild(0).GetChildren().Where(s => s.Name == "cobblestone-pickaxe_013").Count();

		if (count <= 0)
		{
			EmitSignalExit();
		}
	}
}
