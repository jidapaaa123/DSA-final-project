using System.Text;

namespace Realms;

public class Map
{
    public Dictionary<int, List<Edge>> AdjacencyList { get; set; }
    public List<int> Rooms { get; set; }
    private static int numberOfRooms = 10;
    public Stack<string> Treasures = new();
    public int CurrentRoom;

    /// <summary>
    /// Generates Map with rooms filled and connected
    /// </summary>
    public Map()
    {
        AdjacencyList = new();
        Rooms = new();

        // Generate Treasures stack
        List<string> remainingItems = new(GameInfo.ItemsList);
        for (int i = 0; i < GameInfo.ItemsList.Count; i++)
        {
            var item = remainingItems[Random.Shared.Next(0, remainingItems.Count)];
            Treasures.Push(item);

            remainingItems.Remove(item);
        }

        // Generate room numbers
        for (int i = 0; i < numberOfRooms; i++)
        {
            var rand = Random.Shared.Next(200); // random number of 0 - 199
            if (!Rooms.Contains(rand))
            {
                Rooms.Add(rand);
            }
            else
            {
                i--; // repeat again
            }
        }

        // Dictionary entries for each room
        foreach (var room in Rooms)
        {
            AdjacencyList[room] = new();
        }

        // Create adjacencies for each room
        // Each room can to any room that's not itself
        for (int i = 0; i < Rooms.Count; i++)
        {
            int numberOfEdges = Random.Shared.Next(1, 5); // Avoid rooms that connect to nothing
            int roomNum = Rooms[i];
            var currentDestinations = AdjacencyList[roomNum].Select(r => r.Destination);

            for (int j = 0; j < numberOfEdges; j++)
            {
                int index = Random.Shared.Next(0, Rooms.Count);
                int destination = Rooms[index];

                if ((!currentDestinations.Contains(destination)) && (destination != roomNum))
                {
                    AdjacencyList[roomNum].Add(new Edge(destination));
                }
                else
                {
                    j--;
                }
            }
        }

        // Set current room
        CurrentRoom = Rooms[0];
    }

    public void Display()
    {
        foreach (var room in AdjacencyList)
        {
            Console.Write(room.Key + "-> ");
            var builder = new StringBuilder();

            foreach (var edge in room.Value)
            {
                builder.Append($"{edge.Destination}, ");
            }

            Console.WriteLine(builder.ToString());
        }
    }
}

public class Edge
{
    public int Destination;
    public string? RequiredItem;
    public int RequiredStrength;

    public Edge(int destination)
    {
        Destination = destination;

        // chance of requiring strength/item
        var rand = Random.Shared.Next(0, 100);
        bool noRequirement = rand >= 25; // [0, 24] == 25% chance of requiring

        if (noRequirement)
        {
            RequiredItem = null;
            RequiredStrength = 0;
        }
        else
        {
            int index = rand % GameInfo.ItemsList.Count;
            RequiredItem = GameInfo.ItemsList[index];
            RequiredStrength = Random.Shared.Next(1, 5);
        }
    }

    public Edge(int destination, string? requiredItem, int requiredStrength)
    {
        Destination = destination;
        RequiredItem = requiredItem;
        RequiredStrength = requiredStrength;
    }
}