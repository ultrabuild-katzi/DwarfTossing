using DwarfTossing.Item;
using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DwarfTossing.Level.Cave;

public partial class Level : Node3D
{
    private static readonly List<GoalObjectAction> GoalObjectActions =
    [
        new(goal => goal is MainGoal.StoneFall, @"cobblestone-\[StoneFall\]-pickaxe.*", ItemManager.SetCobblestoneMineable),
        new(goal => goal is MainGoal.MiningAttempt, @"cobblestone-\[Exit\]-pickaxe.*", ItemManager.SetCobblestoneMineable),
        new(goal => goal < MainGoal.PreElevator, @"temporary_stone-\[First\]", ItemManager.RemovePhysicallyFromWorld),
        new(goal => goal > MainGoal.Village, @"temporary_stone-\[Mine\]", ItemManager.RemovePhysicallyFromWorld),
        new(goal => goal < MainGoal.Village, @"temporary_stone-\[Village\]", ItemManager.RemovePhysicallyFromWorld),
    ];

    [Export] private GoalController _goalController;
    
    public override void _Ready()
    {
        
        string regex = @"detector_area-\[(.*)\]";
        foreach (Area3D area in ItemManager.FindLevelObjectsWithName<Area3D>(this, regex))
        {
            string goalName = Regex.Match(area.Name.ToString().ToLower(), regex).Groups[1].Value;
            bool tryParse = Enum.TryParse(goalName, true, out MainGoal goal);
            if (!tryParse)
            {
                Console.Error.WriteLine("Cannot map detector area " + goalName + " to a goal.");
                continue;
            }

            area.BodyEntered += body =>
            {
                _goalController.UpdateToState(goal);
            };
        }

    }

    private void OnGoalUpdateObjects(MainGoal currentGoal, MainGoal previousGoal)
    {
        foreach (GoalObjectAction action in GoalObjectActions)
        {
            bool isCurrentlyActive = action.IsActive(currentGoal); 
            bool wasPreviouslyActive = action.IsActive(previousGoal); 
            if (isCurrentlyActive != wasPreviouslyActive)
                continue;

            foreach (MeshInstance3D meshInstance3D in ItemManager.FindLevelObjectsWithName<MeshInstance3D>(this, action.ObjectPattern))
            {
                StaticBody3D staticBody3D = meshInstance3D.GetNode<StaticBody3D>("StaticBody3D");
                
                if (isCurrentlyActive)
                {
                    action.OnActivate(staticBody3D);
                }
                else
                {
                    ItemManager.RestorePhysicallyToWorld(staticBody3D);
                }
            }
        }
    }

    private readonly record struct GoalObjectAction(
        Func<MainGoal, bool> IsActive,
        string ObjectPattern,
        Action<StaticBody3D> OnActivate
    );
}
