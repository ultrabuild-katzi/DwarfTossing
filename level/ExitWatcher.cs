using DwarfTossing.Character.Player;
using Godot;
using System;
using System.Linq;

public partial class ExitWatcher : Node3D
{
	[Export] private Node3D _caveDecorations;
    [Export] private CsgMesh3D _exitDecoration;
	[Signal] public delegate void ExitEventHandler();
	[Signal] public delegate void RestoredEventHandler();

	public override void _PhysicsProcess(double delta)
    {
		int count = _caveDecorations
            .GetChild(0)
            .GetChildren()
            .Count(s => s.Name == "cobblestone_013" && ((Node3D)s).Visible);

		if (count <= 0)
		{
			EmitSignalExit();
		}
	}

    public void Restore()
    {
        foreach (Node node1 in GetNode("../cave_decorations").GetChild(0)
                     .GetChildren().Where(s => s.Name.ToString().StartsWith("cobblestone")))
        {
            var node = (Node3D)node1;
            node.Visible = true;
        }
        EmitSignalRestored();
    }

    public void OnGoalChange(GoalController.MainGoal newGoal)
    {
        if (newGoal == GoalController.MainGoal.RestoreCave)
        {
            Restore();
        }
        
        _exitDecoration.Visible = newGoal == GoalController.MainGoal.MineOutOfCave2;
    }
    
}
