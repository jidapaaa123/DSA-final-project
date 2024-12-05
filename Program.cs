using Realms;

// Generate Character & Map
Hero hero = new();
Map map = new();
Challenges challenges = new();
GameManager game = new GameManager(hero, map, challenges);

Screen.WelcomePlayer();

while (true)
{
    // Game has concluded!!
    if (game.GameEnd)
    {
        break;
    }

    game.Play();
}

// display ending screen
Console.Clear();
Console.WriteLine(game.ResultsMessage);
Screen.DisplayHeroInfo(game.Hero);
map.Display();

