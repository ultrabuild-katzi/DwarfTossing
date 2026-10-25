using DwarfTossing.Character.Player;
using Godot;
using System;
using System.Linq;

public partial class BoomArea : Area3D
{
    [Export] private int _fromIndex;
    [Export] private int _toIndex;
    [Export] private GoalController.MainGoal _activeGoal;
    [Signal] public delegate void ExplodeEventHandler();

    public override void _Ready()
    {
        Monitoring = false;
    }

    public void OnBodyEntered(Node3D body)
    {
        if (body is Player)
        {
            DeleteBarrier();
        }
    }

    public void OnGoalChange(GoalController.MainGoal goal)
    {
        Monitoring = goal == _activeGoal;
    }

    private void DeleteBarrier()
    {
        // Delete one barrier UwU
        var children = GetNode<Node3D>(new("../cave_decorations")).GetChild(0).GetChildren().Where(c =>
            c.Name.ToString().StartsWith("cobblestone_") && int.Parse(c.Name.ToString()[12..15]) >= _fromIndex &&
            int.Parse(c.Name.ToString()[12..15]) <= _toIndex);

        foreach (var child in children)
        {
            (child as Node3D).RemoveChild(child.GetChild(0));
            (child as Node3D).Hide();
        }

        GpuParticles3D particles = GetChild<GpuParticles3D>(1);
        particles.Emitting = true;

        AudioStreamPlayer3D player = GetChild<AudioStreamPlayer3D>(2);
        player.Playing = true;
        EmitSignalExplode();
    }
}
