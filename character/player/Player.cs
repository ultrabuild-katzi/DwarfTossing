using Godot;
using System;

namespace DwarfTossing.Character.Player;

public partial class Player : CharacterBody3D
{
	[Export] public float Speed = 3.5f;
	[Export] public float FollowSpeed = 0.5f;
	[Export] private Camera3D _camera;
	[Export] private AudioStreamPlayer3D _footstepsStream;
	[Export] private Label _targetingLabel;
    [Export] private GoalController _goalController;
    [Export] private Node3D _pickaxeParent;

	private float _cameraBobbing = 10;
	private double _timeSinceStartedMoving;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _PhysicsProcess(double delta)
	{
        HandleMovement(delta);
        HandleInteractions(delta);
	}

    private void HandleMovement(double delta)
    {
        Vector3 velocity = Velocity;
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Vector3 camForward = _camera.GlobalTransform.Basis.Z;
        camForward.Y = 0;
        camForward = camForward.Normalized();
        Vector3 camRight = _camera.GlobalTransform.Basis.X;
        camRight.Y = 0;
        camRight = camRight.Normalized();

        Vector3 direction = (camRight * inputDir.X + camForward * inputDir.Y).Normalized();

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

        if (inputDir != Vector2.Zero)
        {
            _timeSinceStartedMoving += delta;
            _camera.VOffset = (float)Math.Sin(_timeSinceStartedMoving * 10) / 20;

            if (!_footstepsStream.Playing)
                _footstepsStream.Play();
        }
        else
        {
            _timeSinceStartedMoving = 0;
            _camera.VOffset = 0;

            _footstepsStream.Stop();
        }

        MoveAndSlide();
    }

    private void HandleInteractions(double delta)
    {
        
        Vector2 screenCenter = GetViewport().GetVisibleRect().Size / 2;
        Vector3 rayOrigin = _camera.ProjectRayOrigin(screenCenter);
        Vector3 rayEnd = rayOrigin + _camera.ProjectRayNormal(screenCenter) * 2.5f;

        PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;

        var query = PhysicsRayQueryParameters3D.Create(rayOrigin, rayEnd);
        query.CollideWithAreas = true;
        query.CollideWithBodies = true;
        if (!_goalController.HasPickaxeInCurrentState())
            query.CollisionMask = 2; // ONLY OUR ITEMS - YEAH!
        else
            query.CollisionMask = 2 | 4; // OUR ITEMS AND THE MINABLE COBBLESTONE
        

        Godot.Collections.Dictionary hitResult = spaceState.IntersectRay(query);

        if (hitResult.Count <= 0)
        {
            _targetingLabel.Text = "";
            return;
        }

        Node3D collider = (Node3D)hitResult["collider"];
        Node parent = collider.GetParent();
        string name = parent.Name.ToString().ToLower();
        GoalController.Item? item = GoalController.GetItemByModelName(name);
        if (item == null) return;
        
        if (Input.IsActionJustReleased("pickup_item"))
        {
            _goalController.InteractWith(parent, (GoalController.Item)item);
        }
        
        _targetingLabel.Text = $"[${InputMap.GetActionDescription("pickup_item")}] ${GoalController.LookAtDescription[(GoalController.Item)item]}";
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
