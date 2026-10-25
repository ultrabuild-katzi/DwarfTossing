using System.Collections.Generic;
using static DwarfTossing.Character.Player.GoalController;

namespace DwarfTossing.Character.Player;

public static class Translations
{
    public static readonly Dictionary<MainGoal, string> GoalDescriptions = new()
    {
        { MainGoal.Init, "STILL LOADING GAME" },
        { MainGoal.FindCompass, "Find a compass" },
        { MainGoal.PickupCompass, "Pick the compass up" },
        { MainGoal.LookAfterCompass, "Look after the compass" },
        { MainGoal.FindVillage, "Explore the new area" },
        { MainGoal.MineOutOfCave1, "Use the pickaxe to mine the entrance" },
        { MainGoal.TemporaryGoalForce1, "Fulfill your desires" },
        { MainGoal.MineOutOfCave2, "Mine your way free through the entrance" },
        { MainGoal.TemporaryGoalForce2, "Fulfill your desires" },
        { MainGoal.SeeLittleGuy, "Go back to the entrance" }
    };

    public static readonly Dictionary<TemporaryGoal, string> TemporaryGoalDescriptions = new()
    {
        { TemporaryGoal.Drink, "You feel thirsty" },
        { TemporaryGoal.Eat, "You feel hungry" },
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
}
