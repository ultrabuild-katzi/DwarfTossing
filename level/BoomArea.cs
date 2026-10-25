using DwarfTossing.Character.Player;
using Godot;
using System;
using System.Linq;

public partial class BoomArea : Area3D
{
	private bool _hasDied;

	public override void _Process(double delta)
	{
		if (!_hasDied)
		{
			foreach (var body in GetOverlappingBodies())
			{
				if (body is Player)
					DeleteBarrier();
			}
		}
	}

	private void DeleteBarrier()
	{
		// Delete one barrier UwU
		var children = GetNode<Node3D>(new("../cave_decorations")).GetChild(0).GetChildren().Where(c => c.Name.ToString().StartsWith("cobblestone_") && int.Parse(c.Name.ToString()[12..15]) >= 35 && int.Parse(c.Name.ToString()[12..15]) <= 59);

		foreach (var child in children)
		{
			(child as Node3D).RemoveChild(child.GetChild(0));
			(child as Node3D).Hide();
		}

		GpuParticles3D particles = GetChild<GpuParticles3D>(1);
		particles.Emitting = true;

		AudioStreamPlayer3D player = GetChild<AudioStreamPlayer3D>(2);
		player.Playing = true;

		_hasDied = true;
	}
}
