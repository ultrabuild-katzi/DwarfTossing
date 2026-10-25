using Godot;
using System;

public partial class SanityController : ColorRect
{
	private static double _sanity = 0;
	private static double _preferredSanity = 0;

	private double _time = 0;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_sanity = Mathf.Lerp(_sanity, _preferredSanity, 1 * (float)delta);

		_preferredSanity += delta / 20;
		_preferredSanity = Math.Min(_preferredSanity, 2);

		ShaderMaterial material = GetMaterial() as ShaderMaterial;
		material!.SetShaderParameter("random_modifier", (_time += delta) % 1.0);
		material!.SetShaderParameter("grain_amount", _sanity);
	}

	public static void Drink()
	{
		_preferredSanity -= 1.5;
		_preferredSanity = Math.Max(_preferredSanity, 0);
	}

	public static void Eat()
	{
		_preferredSanity -= 0.5;
		_preferredSanity = Math.Max(_preferredSanity, 0);
	}
}
