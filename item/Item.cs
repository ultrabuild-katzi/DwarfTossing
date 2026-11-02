using DwarfTossing.Level.Cave;
using Godot;
using Godot.Collections;
using System;

namespace DwarfTossing.Item;

public partial class Item : Node3D
{
    [Export] private Array<MainGoal> _activeAtGoals;

    private void OnMainGoalChange(MainGoal goal, MainGoal previousGoal)
    {
        if (_activeAtGoals.Contains(goal))
        {
            ItemManager.RestorePhysicallyToWorld(GetChild<PhysicsBody3D>(0));
        }
        else
        {
            ItemManager.RemovePhysicallyFromWorld(GetChild<PhysicsBody3D>(0));
        }
    }
}
