using Godot;

public partial class CharacterController : CharacterBody2D
{
	public const int WALKING_SPEED = 150;
	public const float GRAVITY = 0.09f;
	public const float JUMP_STRENGTH = 3f;

	private float _yVelocity;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 movement = new();
		if (Input.IsKeyPressed(Key.A)) movement.X -= 1;
		if (Input.IsKeyPressed(Key.D)) movement.X += 1;

		if (IsOnFloor())
		{
			_yVelocity = 0;
			if (Input.IsKeyPressed(Key.Space)) _yVelocity -= JUMP_STRENGTH;
		}

		_yVelocity += GRAVITY;

		movement.Y += _yVelocity;

		Velocity += movement * WALKING_SPEED;
		Velocity *= 0.8f;

		MoveAndSlide();
	}
}
