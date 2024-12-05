namespace Realms;


public class Hero
{
    public int HP { get; set; }
    public int Strength { get; set; }
    public int Agility { get; set; }
    public int Intelligence { get; set; }

    public Queue<string> Inventory = new();
    public int CollectedKeys = 0;
    public Stack<int> RoomsHistory = new();

    public Hero()
    {
        // Give him 2 starting items
        var rand1 = Random.Shared.Next(0, GameInfo.ItemsList.Count);
        Inventory.Enqueue(GameInfo.ItemsList[rand1]);
        while (true)
        {
            var rand2 = Random.Shared.Next(0, GameInfo.ItemsList.Count);
            if (rand2 != rand1)
            {
                Inventory.Enqueue(GameInfo.ItemsList[rand2]);
                break;
            }
        }

        // initialize stats
        HP = 200;
        Strength = Random.Shared.Next(0, 5);
        Agility = Random.Shared.Next(0, 5);
        Intelligence = Random.Shared.Next(0, 5);
    }

    public void CollectItem(string? item, out string? deletedItem)
    {
        deletedItem = null;
        if (item == null)
        {
            return;
        }

        if (Inventory.Count == 5) // Inventory already full
        {
            deletedItem = Inventory.Dequeue();
        }

        // Enqueue normally
        Inventory.Enqueue(item);
    }

    public bool CanEnterPath(Edge path)
    {
        string? requiredItem = path.RequiredItem;
        int requiredStrength = path.RequiredStrength;

        return requiredItem == null ? true : (Strength >= requiredStrength || Inventory.Contains(requiredItem));
    }
}
