using Godot;

namespace DwarfTossing.Level.Cave;

public partial class GoalController : Node
{
    public MainGoal MainGoal { get; private set; } = MainGoal.Init;
    private double _mainGoalVisibleForSeconds;

    [Signal]
    public delegate void GoalChangeEventHandler(MainGoal goal, MainGoal previousGoal);

    public override void _Ready()
    {
        UpdateToState(MainGoal.Init + 1);
    }

    public void UpdateToState(MainGoal goal)
    {
        if (MainGoal >= goal)
        {
            return;
        }

        MainGoal previousGoal = MainGoal;

        MainGoal = goal;
        EmitSignalGoalChange(MainGoal, previousGoal);
    }

    public static bool HasPickaxeAtGoal(MainGoal mainGoal)
    {
        return mainGoal is MainGoal.MiningAttempt or MainGoal.StoneFall or MainGoal.TreasureCollected;
    }
}

public enum MainGoal
{
    Init,
    Kill0,
    Scare0,
    Scare1,
    Chase,
    PreElevator,
    Elevator, // triggered by using elevator button
    PostElevator,
    Scare2,
    DeadDwarf,
    RadiationWarning,
    PreVillage,
    Village,
    StoneFall, // triggered by collecting pickaxe in village
    MiningAttempt,
    CollectTreasureMap, // triggered by mining cobblestone
    FindTreasure, // triggered by collecting map
    TreasureCollected,
    TreasureDwarfEncounter, // triggered by mining the main cobblestone
    GoodEnding, // triggered by giving the crystal back to the 
    BadEnding,
}
