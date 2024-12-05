namespace Realms;

public enum Skill { Strength = 0, Agility = 1, Intelligence = 2 }
public enum ChallengeType { Puzzle = 0, Combat = 1, Trap = 2 }

public static class GameInfo
{
    public static List<string> ItemsList = new()
    {
        // 16 items
        "Potion of Strength", "Smoke Bomb", "Puzzle Hint Scroll",
        "Health Elixir", "Combat Charm", "Trap Disarming Kit",
        "Lockpick", "Torch", "Antidote", "Energy Crystal",
        "Compass", "Rope of Escape", "Map Fragment", "Magic Mirror",
        "Silent Cloak", "Enchanted Key",
    };
}