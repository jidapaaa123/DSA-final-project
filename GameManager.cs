namespace Realms;

public record RoomConditions(int RoomNum, Hero Hero, Node Challenge,
                             bool Success, bool GotKey, int Damage, string? Treasure);

public class GameManager
{
    public Hero Hero { get; set; }
    public Map Map { get; set; }
    public Challenges Challenges { get; set; }
    public bool GameEnd { get; set; }
    public bool HasWon { get; set; }
    public string? ResultsMessage { get; set; }

    public GameManager(Hero hero, Map map, Challenges challenges)
    {
        Hero = hero;
        Map = map;
        Challenges = challenges;
        GameEnd = false;
        HasWon = false;
        ResultsMessage = null;
    }

    public void Play()
    {
        Screen.PrintBorder();
        var roomConditions = EnterRoom(Map.CurrentRoom);
        Screen.DisplayRoomEncounter(roomConditions);

        if (GameEnd)
        {
            return;
            // let Program.cs handle the ending screen
        }

        // If game didn't end already,
        // It might just end if there's no take-able path
        EvaluateRoom(roomConditions);
    }

    /// <summary>
    /// Returns initial conditions of the room. Checks winning conditions too
    /// </summary>
    /// <param name="roomNum"></param>
    private RoomConditions EnterRoom(int roomNum)
    {
        Hero.RoomsHistory.Push(roomNum);
        Node challenge = Challenges.FindClosest(roomNum);
        Screen.DisplayRoomEntrance(roomNum, challenge, Hero);
        string requiredItem = challenge.RequiredItem;
        Skill requiredSkill = challenge.RequiredSkill;
        int requiredStat = challenge.SkillPoints;
        string? treasure = null;
        int damage = 0;

        // Can the hero pass it?
        bool success;
        bool gotKey = false;
        if (Hero.Inventory.Contains(requiredItem))
        {
            success = true;
        }
        else
        {
            success = requiredSkill switch
            {
                Skill.Strength => Hero.Strength >= requiredStat,
                Skill.Intelligence => Hero.Intelligence >= requiredStat,
                Skill.Agility => Hero.Agility >= requiredStat,
                _ => throw new NotImplementedException("Invalid Skill Type?")
            };
        }

        // If won: Delete challenge & maybe find treasure and keys
        // Chance for keys = 12%
        if (success)
        {
            Challenges.Delete(challenge);
            var randChance = Random.Shared.Next(0, 100);
            bool getsTreasure = (Map.Treasures.Count != 0) && (randChance < 35); // 35% of getting an item

            if (getsTreasure)
            {
                treasure = Map.Treasures.Pop();
            }

            var randChance2 = Random.Shared.Next(0, 100);
            bool getsKey = randChance2 < 0;
            if (getsKey)
            {
                gotKey = getsKey;
                Hero.CollectedKeys++;
            }
        }
        else
        // increase Hero's stat accordingly
        // take away the HP too
        {

            switch (requiredSkill)
            {
                case Skill.Strength:
                    damage = challenge.SkillPoints - Hero.Strength;
                    Hero.Strength += challenge.SkillPoints / 2;
                    break;
                case Skill.Intelligence:
                    damage = challenge.SkillPoints - Hero.Intelligence;
                    Hero.Intelligence += challenge.SkillPoints / 2;
                    break;
                case Skill.Agility:
                    damage = challenge.SkillPoints - Hero.Agility;
                    Hero.Agility += challenge.SkillPoints / 2;
                    break;
            }

            Hero.HP -= damage;
        }

        // ENDING CONDITION CHECK
        // 3 keys
        if (Hero.CollectedKeys == 3)
        {
            GameEnd = true;
            HasWon = true;
            ResultsMessage = "You found all 3 keys and made it out of the dungeon!";
        }
        else if (Challenges.Count == 0)
        {
            // succumb to darkness
            GameEnd = true;
            HasWon = false;
            ResultsMessage = "You succumbed to the darkness and fell...";
        }
        else if (Hero.HP <= 0)
        {
            GameEnd = true;
            HasWon = false;
            ResultsMessage = "You exhausted yourself a little too hard. Game over...";
        }

        // if didn't fulfill any, then the proceed to the next stages of this room's exploration
        return new RoomConditions(roomNum, Hero, challenge, success, gotKey, damage, treasure);
    }

    private void EvaluateRoom(RoomConditions info)
    {
        // if there is treasure
        if (info.Treasure != null)
        {
            bool collect = Screen.AskToCollectTreasure(info.Treasure);

            if (collect)
            {
                Hero.CollectItem(info.Treasure, out string? deletedItem);
            }
        }

        // Figure out next room to go to
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nUpdated Hero stats: ");
        Console.ForegroundColor = ConsoleColor.Gray;
        Screen.DisplayHeroInfo(Hero);
        List<Edge> options = Map.AdjacencyList[info.RoomNum];
        // Map guarantees every room is connected to at least 1 room that's not itself
        Screen.DisplayRoomOptions(options);

        // has options, but cannot enter any
        if (options.Select(r => Hero.CanEnterPath(r)).ToList().Count == 0)
        {
            GameEnd = true;
            HasWon = false;
            ResultsMessage = "You cannot progress anywhere. You're stuck here forever";
            return;
        }

        // ask for desired room
        int nextRoom = Screen.AskRoomOption(options);
        Map.CurrentRoom = nextRoom;
        return;
    }
}