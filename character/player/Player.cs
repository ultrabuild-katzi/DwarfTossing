using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [Export]
    public float Speed = 5.0f;

    [Export]
    public float FollowSpeed = 0.5f;

    private Camera3D _camera;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        _camera = GetNode<Camera3D>("Camera3D");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        Vector3 forward = _camera.GlobalTransform.Basis.Z;
        forward.Y = 0;
        forward = forward.Normalized();

        Vector3 right = _camera.GlobalTransform.Basis.X;
        right = right.Normalized();

        Vector3 movement = new();
        if (Input.IsKeyPressed(Key.W)) movement -= forward * Speed;
        if (Input.IsKeyPressed(Key.S)) movement += forward * Speed;
        if (Input.IsKeyPressed(Key.A)) movement -= right * Speed;
        if (Input.IsKeyPressed(Key.D)) movement += right * Speed;


        velocity = new Vector3(movement.X, 0, movement.Z);

        //if (direction != Vector3.Zero)
        //{
        //    velocity.X = direction.X * Speed;
        //    velocity.Z = direction.Z * Speed;
        //}
        //else
        //{
        //    velocity.X = Mathf.MoveToward(Velocity.X, 0, 0.5f);
        //    velocity.Z = Mathf.MoveToward(Velocity.Z, 0, 0.5f);
        //}

        Velocity = velocity;
        MoveAndSlide();
    }

    public override void _Input(InputEvent motionUnknown)
    {
        if (motionUnknown is InputEventMouseMotion mouseMotion)
        {
            Vector2 mouseVelocity = mouseMotion.Relative;
            Vector3 localDirection = new Vector3(-mouseVelocity.Y, -mouseVelocity.X, 0) / 4;
            _camera.Rotation += localDirection / 50;

            _camera.Rotation = new Vector3(Math.Clamp(_camera.Rotation.X, -1.0f, 1.0f), _camera.Rotation.Y, _camera.Rotation.Z);
        }
    }
}
