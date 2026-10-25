using Godot;
using GodotPlugins.Game;
using System;
using System.Collections.Generic;

namespace DwarfTossing.Character.Player;

public partial class GoalController : Node
{
    private MainGoal _mainGoal;
    private TemporaryGoal? _temporaryGoal;
    private double _temporaryGoalTimeoutSeconds;
    private double _mainGoalVisibleForSeconds;

    [Export] private Player _player;
    [Export] private SanityController _sanityController;
    [Export] private Label _mainGoalLabel;
    [Export] private Label _temporaryGoalLabel;
    [Export] private double _hintSeconds = 120;

    private readonly Random _random = new();
    
    private static readonly Dictionary<string, Item> ItemModelNames = new()
    {
        { "water", Item.Water },
        { "poisonous_mushroom", Item.PoisonousMushroom },
        { "eatable_mushroom", Item.EatableMushroom },
        { "pickaxe", Item.Pickaxe },
        { "compass", Item.Compass },
        { "cobblestone", Item.Cobblestone },
    };

    public override void _Ready()
    {
        Progress();
        _mainGoalVisibleForSeconds = 0;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_sanityController._sanity > 0 && _temporaryGoal == null && _temporaryGoalTimeoutSeconds <= 0)
        {
            _temporaryGoal = _random.GetItems(Enum.GetValues<TemporaryGoal>(), 1)[0];
        }

        if (_temporaryGoalTimeoutSeconds > 0)
            _temporaryGoalTimeoutSeconds -= delta;

        _mainGoalVisibleForSeconds += delta;
        _mainGoalLabel.Text = Translations.GoalDescriptions[_hintSeconds < 0 ? _mainGoal - 1 : _mainGoal];
        InterpolateAlpha(_mainGoalLabel, _mainGoalVisibleForSeconds > _hintSeconds ? 1 : 0, delta * 2);

        if (_temporaryGoal != null)
            _temporaryGoalLabel.Text = Translations.TemporaryGoalDescriptions[(TemporaryGoal)_temporaryGoal];
        InterpolateAlpha(_temporaryGoalLabel, _temporaryGoal == null ? 0 : 1, delta * 2);
    }

    public void InterpolateAlpha(Label label, int toAlpha, double weight)
    {
        Color color = label.Modulate;
        color.A = (float)Mathf.Lerp(color.A, toAlpha, weight);
        label.Modulate = color;
    }

    public void Progress()
    {
        _mainGoal++;
        _mainGoalVisibleForSeconds = -1;

        GetNode<Sprite3D>("../Camera3D/Pickaxe").Visible = HasPickaxeInCurrentState();
        GetNode<Sprite3D>("../Camera3D/Compass").Visible = _mainGoal == MainGoal.LookAfterCompass;
        _player.EmitGoalChange(_mainGoal);
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

    public enum MainGoal
    {
        Init,
        FindCompass,
        PickupCompass,
        LookAfterCompass,
        FindVillage,
        MineOutOfCave1,
        TemporaryGoalForce1,
        MineOutOfCave2,
        TemporaryGoalForce2,
        SeeLittleGuy
    }

    public enum TemporaryGoal
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
