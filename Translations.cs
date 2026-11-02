using DwarfTossing.Character.Player;
using DwarfTossing.Item;
using System.Collections.Generic;

namespace DwarfTossing;

public static class Translations
{
    // public static readonly Dictionary<MainGoal, string> GoalDescriptions = new()
    // {
    //     { MainGoal.Init, "STILL LOADING GAME" },
    //     { MainGoal.FindCompass, "Find a compass" },
    //     { MainGoal.PickupCompass, "Pick the compass up" },
    //     { MainGoal.LookAfterCompass, "Look after the compass" },
    //     { MainGoal.FindVillage, "Explore the new area" },
    //     { MainGoal.MineOutOfCave1, "Use the pickaxe to mine the entrance" },
    //     { MainGoal.TemporaryGoalForce1, "Fulfill your desires" },
    //     { MainGoal.MineOutOfCave2, "Mine your way free through the entrance" },
    //     // { MainGoal.TemporaryGoalForce2, "Fulfill your desires" },
    //     { MainGoal.SeeLittleGuy, "Go back to the entrance" }
    // };

    public static readonly Dictionary<TemporaryGoal, string> TemporaryGoalDescriptions = new()
    {
        { TemporaryGoal.Drink, "You feel thirsty" },
        { TemporaryGoal.Eat, "You feel hungry" },
    };

    public static readonly Dictionary<Items, string> LookAtDescription = new()
    {
        { Items.Water, "Drink Water" },
        { Items.PoisonousMushroom, "Eat Mushroom" },
        { Items.EatableMushroom, "Eat Mushroom" },
        { Items.Pickaxe, "Pickup Pickaxe" },
        { Items.Map, "Pickup Treasure Map" },
        { Items.Crystal, "Pickup Crystal" },
        { Items.ElevatorButton, "Activate Elevator" },
        { Items.Cobblestone, "Mine Cobblestone" },
    };
}
