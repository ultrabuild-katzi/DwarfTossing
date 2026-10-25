using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DwarfTossing.Character.Player;

public partial class Player : CharacterBody3D
{
	[Export]
	public float Speed = 3.5f;

	[Export]
	public float FollowSpeed = 0.5f;

	[Export]
	private Camera3D _camera;

	[Export]
	private AudioStreamPlayer3D _footstepsStream;

	[Export] private Label _targetingLabel;

	[Export]
	private StaticBody3D[] _excludeFromRaycasting;

	private bool _hasPickaxe = false;

	private float _cameraBobbing = 10;
	private double _elapsedTime;

	private Node3D _pickaxeParent;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		_pickaxeParent = GetNode<Node3D>("../PickaxeParent");
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
			_elapsedTime += delta;
			_camera.VOffset = (float)Math.Sin(_elapsedTime * 10) / 20;

			if (!_footstepsStream.Playing)
				_footstepsStream.Play();
		}
		else
		{
			_elapsedTime = 0;
			_camera.VOffset = 0;

			_footstepsStream.Stop();
		}

		MoveAndSlide();

		Vector2 screenCenter = GetViewport().GetVisibleRect().Size / 2;
		Vector3 rayOrigin = _camera.ProjectRayOrigin(screenCenter);
		Vector3 rayEnd = rayOrigin + _camera.ProjectRayNormal(screenCenter) * 2.5f;

		PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;

		var query = PhysicsRayQueryParameters3D.Create(rayOrigin, rayEnd);
		query.Exclude = [GetRid()];
		query.CollideWithAreas = true;
		query.CollideWithBodies = true;
		query.CollisionMask = 2 | 4; // ONLY OUR ITEMS - YEAH!

		Godot.Collections.Dictionary hitResult = spaceState.IntersectRay(query);


		if (hitResult.Count <= 0)
		{
			_targetingLabel.Text = $"Hit object: None";
			return;
		}

		Node3D collider = (Node3D)hitResult["collider"];
		string name = collider.Name.ToString();

		if (Input.IsActionJustReleased("pickup_item"))
		{
			if (name.StartsWith("water"))
			{

			}

			if (name.Equals("PickaxeParent"))
			{
				PickupPickaxe();
				var world = collider.GetParent();
				world.RemoveChild(collider);
			}
		}


		_targetingLabel.Text = $"Hit object: {name}";

	}

	public void PickupPickaxe()
	{
		_hasPickaxe = true;
		GetNode<Sprite3D>("Camera3D/Pickaxe").Visible = true;
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
