using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [Export]
    public float Speed = 5.0f;
    
    [Export]
    public float FollowSpeed = 0.5f;

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
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

        Node3D cameraController = GetNode<Node3D>("CameraController");
        
        cameraController.Position = cameraController.Position.Lerp(Position, FollowSpeed);
    }
}
