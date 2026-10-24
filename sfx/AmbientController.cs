using Godot;
using System;

public partial class AmbientController : AudioStreamPlayer3D
{
    [Export]
    public AudioStream[] AudioStreams;

    private const int COOLDOWN_BASE = 40;
    private const int COOLDOWN_RANDOM = 20;

    private float _cooldown;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Random random = new();
        Stream = AudioStreams[random.Next(0, AudioStreams.Length)];
        Play();

        ResetCooldown();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        _cooldown -= (float)delta;

        if (_cooldown <= 0)
        {
            Random random = new();
            Stream = AudioStreams[random.Next(0, AudioStreams.Length)];
            Play();
            ResetCooldown();
        }
    }

    private void ResetCooldown()
    {
        Random random = new();
        _cooldown = COOLDOWN_BASE + random.Next(-COOLDOWN_RANDOM, COOLDOWN_RANDOM);
    }
}
