using Godot;
using System;

public partial class SanityController : ColorRect
{
	private double _sanity = 0;
	private double _time = 0;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_sanity += delta / 45;
		_sanity = Math.Min(_sanity, 2);

		ShaderMaterial material = GetMaterial() as ShaderMaterial;
		material!.SetShaderParameter("random_modifier", (_time += delta) % 1.0);
		material!.SetShaderParameter("grain_amount", _sanity);
	}
}
