using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DwarfTossing.Item;

public static class ItemManager
{
    private static readonly Dictionary<string, Items> ItemModelNames = new()
    {
        { "water", Items.Water },
        { "poisonous_mushroom", Items.PoisonousMushroom },
        { "eatable_mushroom", Items.EatableMushroom },
        { "pickaxe", Items.Pickaxe },
        { "map", Items.Map },
        { "elevator_button", Items.ElevatorButton },
        { "crystal", Items.Crystal },
        { "cobblestone", Items.Cobblestone },
    };

    public static Items? GetItemByModelName(string targetModelName)
    {
        foreach ((string modelName, Items item) in ItemModelNames)
        {
            if (targetModelName.StartsWith(modelName))
                return item;
        }

        return null;
    }

    public static IEnumerable<T> FindLevelObjectsWithName<T>(Node3D level, string pattern)
    {
        return level
            .GetChildren()
            .Where(s => Regex.IsMatch(s.Name.ToString(), pattern))
            .Cast<T>();
    }

    public static void SetCobblestoneMineable(PhysicsBody3D cobblestone)
    {
        cobblestone.SetCollisionLayer(1 | 4);
        cobblestone.SetCollisionMask(1);
    }

    public static void SetItemVisible(StaticBody3D item)
    {
        item.SetCollisionLayer(1);
    }

    public static void RestorePhysicallyToWorld(PhysicsBody3D body)
    {
        body.GetParentOrNull<MeshInstance3D>()?.Show();
        
        body.SetCollisionLayer(1);
        body.SetCollisionMask(1);
        body.Show();
        body.SetPhysicsProcess(true);
    }
    
    public static void RemovePhysicallyFromWorld(PhysicsBody3D body)
    {
        body.GetParentOrNull<MeshInstance3D>()?.Hide();

        body.SetCollisionLayer(0);
        body.SetCollisionMask(0);
        body.Hide();
        body.SetPhysicsProcess(false);
    }
}

public enum Items
{
    Water,
    PoisonousMushroom,
    EatableMushroom,
    Pickaxe,
    Map,
    Crystal,
    ElevatorButton,
    Cobblestone
}
