using Godot;
using System;

public partial class SanityController : ColorRect
{
	public double _sanity = 0;
	private static double _sanityTransition = 0;

	private double _time = 0;

	public override void _Process(double delta)
	{
		_sanityTransition = Mathf.Lerp(_sanityTransition, Math.Clamp(_sanityTransition, 0, 2), 1 * (float)delta);
		_sanityTransition = Math.Min(_sanity, 2);

		ShaderMaterial material = GetMaterial() as ShaderMaterial;
		material!.SetShaderParameter("random_modifier", (_time += delta) % 1.0);
		material!.SetShaderParameter("grain_amount", _sanityTransition);
		
		_sanity += delta / 60;
	}
}
