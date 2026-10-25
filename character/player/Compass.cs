using Godot;
using System;

public partial class Compass : Sprite3D
{
	public override void _Process(double delta)
	{
		LookAt(new Vector3(-18.6f, 3.85f, 8.1f) + new Vector3(3.6f, 0.5f, 3.3f), null, true);
	}
}
