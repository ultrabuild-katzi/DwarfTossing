using Godot;
using System;

public partial class SanityController : ColorRect
{
	public double _sanity = 0;

	private double _time = 0;

	public override void _Process(double delta)
	{
		ShaderMaterial material = GetMaterial() as ShaderMaterial;
		material!.SetShaderParameter("random_modifier", (_time += delta) % 1.0);
		material!.SetShaderParameter("grain_amount", Math.Clamp(_sanity, 0, 2));
        
        _sanity += delta / 60;
	}
}
