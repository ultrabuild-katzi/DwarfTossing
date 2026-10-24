using Godot;
using System;

public partial class SanityController : ColorRect
{
	[Export]
	public float Sanity;

    private double _time;

	public override void _Ready()
	{
        Console.WriteLine("SanityController::_Ready");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        
        ShaderMaterial material = GetMaterial() as ShaderMaterial;
        material!.SetShaderParameter("random_modifier", (_time += delta) % 1.0);
        material!.SetShaderParameter("grain_amount", Sanity);
    }
}
