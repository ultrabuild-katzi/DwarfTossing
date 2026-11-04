using Godot;
using System;
using DwarfTossing.Item;
using DwarfTossing.Level.Cave;

namespace DwarfTossing.Character.Player;

public partial class PlayerGoalController : Node
{
    private TemporaryGoal? _temporaryGoal;
    private double _temporaryGoalTimeoutSeconds;
    
    [Export] public required Player Player;
    [Export] public required Label TemporaryGoalLabel;
	[Export] public required SanityController SanityController;

    private MainGoal _mainGoal;
    
	private readonly Random _random = new();

    public override void _PhysicsProcess(double delta)
    {
        if (_temporaryGoal == null && SanityController._sanity > 0 && _temporaryGoalTimeoutSeconds <= 0)
        {
            _temporaryGoal = _random.GetItems(Enum.GetValues<TemporaryGoal>(), 1)[0];
        }

        if (_temporaryGoalTimeoutSeconds > 0)
            _temporaryGoalTimeoutSeconds -= delta;

        if (_temporaryGoal != null)
            TemporaryGoalLabel.Text = Translations.TemporaryGoalDescriptions[(TemporaryGoal) _temporaryGoal];
        InterpolateAlpha(TemporaryGoalLabel, _temporaryGoal == null ? 0 : 1, delta * 2);
    }

    public void InteractWith(Node itemInstance, Items item)
    {
        switch (item)
        {
            case Items.Water:
                MeshInstance3D waterInstance = (MeshInstance3D) itemInstance;

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
            case Items.PoisonousMushroom or Items.EatableMushroom:
                itemInstance.GetParent().RemoveChild(itemInstance);
                Consume(item);
                break;

            case Items.Pickaxe or Items.Map or Items.ElevatorButton or Items.Crystal:
                
                // itemInstance.GetParent().RemoveChild(itemInstance);
                break;

            case Items.Cobblestone:
                ItemManager.RemovePhysicallyFromWorld((StaticBody3D) itemInstance);
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Consume(Items item)
    {
        if (item == Items.PoisonousMushroom)
        {
            SanityController._sanity = Math.Max(SanityController._sanity, 1.5);
            _temporaryGoal = null;
        }

        if (_temporaryGoal == TemporaryGoal.Drink && item == Items.Water
            || _temporaryGoal == TemporaryGoal.Eat && item == Items.EatableMushroom)
        {
            SanityController._sanity = Math.Max(SanityController._sanity - 1.5, 0);
            _temporaryGoal = null;
        }

        _temporaryGoalTimeoutSeconds = 20;
    }

    private void InterpolateAlpha(Label label, int toAlpha, double weight)
    {
        Color color = label.Modulate;
        color.A = (float)Mathf.Lerp(color.A, toAlpha, weight);
        label.Modulate = color;
    }

    public void OnMainGoalChange(MainGoal mainGoal)
    {
        _mainGoal = mainGoal;
        GetNode<Node3D>("../Camera3D/Pickaxe").Visible = GoalController.HasPickaxeAtGoal(mainGoal);
        GetNode<Node3D>("../Camera3D/Map").Visible = mainGoal is MainGoal.FindTreasure;
        GetNode<Node3D>("../Camera3D/Crystal").Visible = mainGoal is MainGoal.TreasureCollected or MainGoal.TreasureDwarfEncounter or MainGoal.BadEnding;
    }
}

public enum TemporaryGoal
{
    Drink,
    Eat
}
