using System.Text;

namespace Realms;

public static class Screen
{
    /// <summary>
    /// First screen. Explains the game roughly
    /// </summary>
    public static void WelcomePlayer()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the Realms of Data Structures and Algorithms!");
        Console.WriteLine("You will need to survive a set of challenges as you traverse through the map");
        Console.WriteLine("\nThe objective is very simple: ");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Collect 3 Golden Keys before you die");
        Console.ForegroundColor = ConsoleColor.Gray;

        Console.WriteLine("\nEnjoy!");

        EndScreen();
    }

    /// <summary>
    /// Ends current screen after a keypress
    /// </summary>
    /// <param name="message"></param>
    public static void EndScreen(string message = "\nPress any key to continue")
    {
        Console.WriteLine(message);
        Console.ReadKey(true);
    }

    public static void DisplayRoomEntrance(int roomNum, Node challenge, Hero hero)
    {
        Console.WriteLine($"Room#{roomNum} | Challenge#{challenge.Data}");
        DisplayHeroInfo(hero);
    }

    /// <summary>
    /// Top border
    /// </summary>
    public static void PrintBorder()
    {
        int width = Console.WindowWidth;
        var builder = new StringBuilder();
        for (int i = 0; i < width; i++)
        {
            builder.Append('=');
        }
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(builder);
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    public static void DisplayHeroInfo(Hero hero)
    {
        Console.WriteLine($"Keys found: {hero.CollectedKeys}  | HP: {hero.HP}");
        Console.WriteLine($"Strength : {hero.Strength}   | Intelligence: {hero.Intelligence}    | Agility: {hero.Agility}");

        Console.WriteLine("\nInventory:");
        var builder = new StringBuilder();
        builder.Append("[");
        foreach (var item in hero.Inventory)
        {
            builder.Append($" {item} |");
        }
        builder.Append("]");
        Console.WriteLine(builder);
    }

    public static void DisplayRoomEncounter(RoomConditions info)
    {
        Console.WriteLine($"\nIt's a {info.Challenge.Type}!" +
                           "\nYou need");

        Console.WriteLine($"  \u25A0 {info.Challenge.SkillPoints} {info.Challenge.RequiredSkill}");
        Console.WriteLine("OR");
        Console.WriteLine($"  \u25A0 {info.Challenge.RequiredItem}");

        string successMsg = info.Success ? "And you overcame it!" : $"...So you took {info.Damage} damage";
        Console.WriteLine($"\n{successMsg}");

        string treasureMsg = info.Treasure == null ?
                            "Sadly, you found cabbage at the end of the hall" :
                            $"There was a {info.Treasure} in this room's chest!";
        Console.WriteLine($"\n{treasureMsg}");

        string keyMsg = info.GotKey ? "You found a key!" : "No key found";
        Console.WriteLine($"{keyMsg}");
    }

    public static bool AskToCollectTreasure(string treasure)
    {
        while (true)
        {
            Console.WriteLine($"\nWould you like to collect {treasure}? (Y/N): ");
            var input = Console.ReadKey().KeyChar.ToString();

            if (input.ToLower() == "y" || input.ToLower() == "n")
            {
                Console.WriteLine();
                return input.ToLower() == "y";
            }
            else
            {
                Console.WriteLine("Please either press Y or N");
            }
        }

    }

    public static void DisplayRoomOptions(List<Edge> options)
    {
        Console.WriteLine($"\nHere are your possible paths: ");

        foreach (var path in options)
        {
            string requirements = path.RequiredItem == null ?
                                    "No requirements" :
                                    $"{path.RequiredItem} or {path.RequiredStrength} Strength";
            Console.WriteLine($"{path.Destination} -> {requirements}");
        }
    }

    public static int AskRoomOption(List<Edge> options)
    {
        while (true)
        {
            Console.Write("\nEnter the desired room number: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out int num))
            {
                Console.WriteLine("Please enter  a number");
            }
            else if (!options.Select(e => e.Destination).Contains(num))
            {
                Console.WriteLine("Not an option");
            }
            else
            {
                return num;
            }
        }
    }

}