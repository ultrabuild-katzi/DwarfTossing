using Godot;
using GodotPlugins.Game;
using System;
using System.Collections.Generic;

namespace DwarfTossing.Character.Player;

public partial class GoalController : Node
{
    private MainGoal _mainGoal = 0;
    private TemporaryGoal? _temporaryGoal = null;
    private double _temporaryGoalTimeoutSeconds = 0;
    private double _mainGoalVisibleForSeconds;

    [Export] private SanityController _sanityController;

    [Export] private Label _mainGoalLabel;
    [Export] private Label _temporaryGoalLabel;

    private readonly Random _random = new Random();

    private static readonly Dictionary<MainGoal, string> GoalDescriptions = new()
    {
        { MainGoal.FindCompass, "Find a compass" },
        { MainGoal.LookAfterCompass, "Look after the compass" },
        { MainGoal.FindVillage, "Explore the new area" },
        { MainGoal.MineOutOfCave1, "Use the pickaxe to mine the entrance" },
        { MainGoal.TemporaryGoalForce1, "Fulfill your desires" },
        { MainGoal.MineOutOfCave2, "Mine your way free through the entrance" },
        { MainGoal.TemporaryGoalForce2, "Fulfill your desires" },
        { MainGoal.SeeLittleGuy, "Go back to the entrance" }
    };

    private static readonly Dictionary<string, Item> ItemModelNames = new()
    {
        { "water", Item.Water },
        { "poisonous_mushroom", Item.PoisonousMushroom },
        { "eatable_mushroom", Item.EatableMushroom },
        { "pickaxe", Item.Pickaxe },
        { "compass", Item.Compass },
        { "cobblestone", Item.Cobblestone },
    };

    public static readonly Dictionary<Item, string> LookAtDescription = new()
    {
        { Item.EatableMushroom, "Eat Mushroom" },
        { Item.PoisonousMushroom, "Eat Mushroom" },
        { Item.Water, "Drink Water" },
        { Item.Compass, "Pickup Compass" },
        { Item.Pickaxe, "Pickup Pickaxe" },
        { Item.Cobblestone, "Mine Cobblestone" },
    };

    public override void _PhysicsProcess(double delta)
    {
        if (_sanityController._sanity > 0 && _temporaryGoal == null && _temporaryGoalTimeoutSeconds <= 0)
        {
            _temporaryGoal = _random.GetItems(Enum.GetValues<TemporaryGoal>(), 1)[0];
        }

        _temporaryGoalTimeoutSeconds -= delta;
        _mainGoalVisibleForSeconds += delta;

        if (_mainGoalVisibleForSeconds < 0)
        {
            _mainGoalLabel.Text = GoalDescriptions[_mainGoal - 1];

            Color color = _mainGoalLabel.Modulate;
            color.A = (float)(1 - _mainGoalVisibleForSeconds / 2);
            _mainGoalLabel.Modulate = color;
        }
        else if (_mainGoalVisibleForSeconds is > 60 and < 62)
        {
            _mainGoalLabel.Text = GoalDescriptions[_mainGoal];

            Color color = _mainGoalLabel.Modulate;
            color.A = (float)((_mainGoalVisibleForSeconds - 60) / 2);
            _mainGoalLabel.Modulate = color;
        }
    }

    public void Progress()
    {
        _mainGoal++;
        _mainGoalVisibleForSeconds = -1;

        GetNode<Sprite3D>("../Camera3D/Pickaxe").Visible = HasPickaxeInCurrentState();
        GetNode<Sprite3D>("../Camera3D/Compass").Visible = _mainGoal == MainGoal.LookAfterCompass;
    }

    public void InteractWith(Node itemInstance, Item item)
    {
        switch (item)
        {
            case Item.Water:
                MeshInstance3D waterInstance = itemInstance as MeshInstance3D;

                Vector3 scale = waterInstance!.Scale;
                scale.Z -= 0.2f;
                if (scale.Z <= 0)
                {
                    waterInstance.GetParent().RemoveChild(waterInstance);
                }
                else
                {
                    waterInstance.Scale = scale;
                }

                Consume(item);
                break;
            case Item.PoisonousMushroom or Item.EatableMushroom:
                itemInstance.GetParent().RemoveChild(itemInstance);
                Consume(item);
                break;

            case Item.Pickaxe or Item.Compass:
                Progress();
                itemInstance.GetParent().RemoveChild(itemInstance);
                break;
            
            case Item.Cobblestone:
                // Todo break block
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public bool HasPickaxeInCurrentState()
    {
        return _mainGoal >= MainGoal.MineOutOfCave1;
    }

    private void Consume(Item item)
    {
        if (item == Item.PoisonousMushroom)
        {
            _sanityController._sanity = Math.Max(_sanityController._sanity, 1.5);
            _temporaryGoal = null;
        }

        if (_temporaryGoal == TemporaryGoal.Drink && item == Item.Water
            || _temporaryGoal == TemporaryGoal.Eat && item == Item.EatableMushroom)
        {
            _sanityController._sanity = Math.Max(_sanityController._sanity - 1.5, 0);
            _temporaryGoal = null;
        }

        _temporaryGoalTimeoutSeconds = 20;
    }

    public static Item? GetItemByModelName(string targetModelName)
    {
        foreach ((string modelName, Item item) in ItemModelNames)
        {
            if (targetModelName.StartsWith(modelName))
                return item;
        }

        return null;
    }

    enum MainGoal
    {
        FindCompass,
        LookAfterCompass,
        FindVillage,
        MineOutOfCave1,
        TemporaryGoalForce1,
        MineOutOfCave2,
        TemporaryGoalForce2,
        SeeLittleGuy
    }

    enum TemporaryGoal
    {
        Drink,
        Eat
    }

    public enum Item
    {
        Water,
        PoisonousMushroom,
        EatableMushroom,
        Pickaxe,
        Compass,
        Cobblestone
    }
}
