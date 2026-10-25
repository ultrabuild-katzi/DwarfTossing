using Godot;
using System;

public partial class StartGameButton : Button
{
	public override void _Pressed()
	{
		GetTree().ChangeSceneToFile("./level/world.tscn");
	}
}
