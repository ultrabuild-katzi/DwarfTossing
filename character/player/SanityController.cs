using Godot;
using System;

public partial class SanityController : ColorRect
{
	[Export]
	public float SanityDecrease = 10;

	private Vector2 _startPosition;
	private Vector2 _startSize;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_startPosition = Position;
		_startSize = Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Size = new(Size.X - (SanityDecrease * 2) * (float)delta, Size.Y);
		Position = new(Position.X + SanityDecrease * (float)delta, Position.Y);
	}
}
