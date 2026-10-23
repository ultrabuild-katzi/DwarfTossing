using Godot;
using System;

public partial class CameraFollow : Camera3D
{
	[Export]
	public float FollowSpeed = 100;

	private CharacterBody3D _player;
	private static readonly Vector3 _offset = new(0, 2, 3);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player = GetNode<CharacterBody3D>(new("../player"));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = Position.Lerp(_player.Position + _offset, FollowSpeed * (float)delta);
	}
}
