
using Godot;

namespace DwarfTossing.Vfx;

public class Particles 
{
    public static void SpawnParticles(Node3D node3D)
    {
        GpuParticles3D particles = node3D.GetChild<GpuParticles3D>(1);
        particles.Emitting = true;

        AudioStreamPlayer3D player = node3D.GetChild<AudioStreamPlayer3D>(2);
        player.Playing = true;
    }
}
