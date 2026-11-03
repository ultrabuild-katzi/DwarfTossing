using DwarfTossing.Item;
using DwarfTossing.Level.Cave;
using Godot;
using Godot.Collections;
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
    [Export] private PlayerGoalController _playerGoalController;

    private readonly GameSettings _gameSettings = new(1.0f, 1.0f, 1.0f);
    
    private float _cameraBobbing = 10;
    private double _timeSinceStartedMoving;

    // [Export] private CanvasLayer _menu;

    private double _gameQuitTimer;

    
    
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Process(double delta)
    {
        Vector2 joyStickLookDir = Input.GetVector("look_left", "look_right", "look_up", "look_down");
        if (joyStickLookDir != Vector2.Zero)
        {
            const float lookSensitivity = 2f;

            Vector3 rotation = _camera.Rotation;
            rotation.Y += -joyStickLookDir.X * lookSensitivity * (float)delta;
            rotation.X += -joyStickLookDir.Y * lookSensitivity * (float)delta;
            rotation.X = Math.Clamp(rotation.X, -1.0f, 1.0f);
            _camera.Rotation = rotation;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        // if (_ending)
        // {
        //     _camera.LookAt(_dwarf.GlobalPosition);
        //     _dwarf.Show();
        //     _gameQuitTimer += delta;
        //     if (_gameQuitTimer >= 5)
        //     {
        //         GetTree().Quit();
        //     }
        //     return;
        // }
        //
        if (Input.IsActionJustReleased("pause_game"))
        {
            // _paused = !_paused;
            // if (_paused)
            // {
            //     _menu.Show();
            //     Input.MouseMode = Input.MouseModeEnum.Confined;
            // }
            // else
            // {
            //     _menu.Hide();
            //     Input.MouseMode = Input.MouseModeEnum.Captured;
            // }
        }

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
        if (GoalController.HasPickaxeAtGoal(_goalController.MainGoal))
            query.CollisionMask = 2; // ONLY OUR ITEMS - YEAH!
        else
            query.CollisionMask = 2 | 4; // OUR ITEMS AND THE MINABLE COBBLESTONE

        Dictionary hitResult = spaceState.IntersectRay(query);

        if (hitResult.Count <= 0)
        {
            _targetingLabel.Text = "";
            return;
        }

        Node3D collider = (Node3D)hitResult["collider"];
        Node parent = collider.GetParent();
        string name = parent.Name.ToString().ToLower();
        Items? item = ItemManager.GetItemByModelName(name);
        if (item == null) return;

        // if (Input.IsMouseButtonPressed(MouseButton.Left))
        // {
        //     if (collider is RigidBody3D rigidBody3D)
        //     {
        //         Vector3 vector3 = (GlobalPosition - rigidBody3D.GlobalPosition).Normalized() * 1.5f;
        //         vector3.Y = 3;
        //         rigidBody3D.LinearVelocity = vector3;
        //     }
        // }
        if (Input.IsActionJustReleased("pickup_item"))
        {
            _playerGoalController.InteractWith(parent, (Items)item);
        }

        _targetingLabel.Text = $"[{InputMap.GetActionDescription("pickup_item")}] {Translations.LookAtDescription[(Items)item]}";
    }

    private void OnMainGoalChange(MainGoal mainGoal, MainGoal previousGoal)
    {
        Console.WriteLine("MAIN GOAL CHANGE " + mainGoal + " MAIN " + previousGoal);
        _playerGoalController.OnMainGoalChange(mainGoal);
    }

    public override void _Input(InputEvent motionUnknown)
    {
        if (motionUnknown is InputEventMouseMotion mouseMotion)
        {
            Vector2 mouseVelocity = mouseMotion.Relative;
            Vector3 localDirection = new Vector3(-mouseVelocity.Y, -mouseVelocity.X, 0) / 4;
            _camera.Rotation += localDirection / Math.Abs(100f - _gameSettings.MouseSensitivity);
            _camera.Rotation = new Vector3(Math.Clamp(_camera.Rotation.X, -1.0f, 1.0f), _camera.Rotation.Y, _camera.Rotation.Z);
        }
    }
}
