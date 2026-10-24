using Godot;
using System;

namespace DwarfTossing.Character.Player;

public partial class Player : CharacterBody3D
{
    [Export]
    public float Speed = 3.8f;

    [Export]
    public float FollowSpeed = 0.5f;

    [Export]
    private Camera3D _camera;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Vector3 direction = (_camera.GlobalTransform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, 0.5f);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, 0.5f);
        }

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
