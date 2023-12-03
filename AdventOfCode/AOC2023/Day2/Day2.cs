using System.Text.RegularExpressions;
using Utilities;

namespace AOC2023.Day2;

public class Day2
{
    public static void Run()
    {
        string path = FileUtilities.GetProjectSolutionPath();
        path += @"\AOC2023\Day2\input";
        List<string> input = FileUtilities.GetListFromFile(path);

        int sum = GetSumOfIds(input);
        Console.WriteLine("Sum of ids: " + sum);
        int sum2 = GetSumOfPower(input);
        Console.WriteLine("Sum of power: " + sum2);
    }

    public class Game
    {
        public int GameId { get; set; }
        public int RedCubes { get; set; }
        public int GreenCubes { get; set; }
        public int BlueCubes { get; set; }
        public bool IsPossible { get; set; }

        public Game(int id, int redCubes, int greenCubes, int blueCubes, bool isPossible)
        {
            GameId = id;
            RedCubes = redCubes;
            GreenCubes = greenCubes;
            BlueCubes = blueCubes;
            IsPossible = isPossible;
        }
    }

    static Game gameConfig = new Game(0, 12, 13, 14, true);

    public static int GetSumOfIds(List<string> input)
    {
        List<Game> possibleGames = new List<Game>();
        foreach (string line in input)
        {
            Game game = ParseLineToGame(line);
            if (game.IsPossible)
            {
                possibleGames.Add(game);
            }
        }

        return possibleGames.Select(game => game.GameId).Sum();
    }

    public static Game ParseLineToGame(string line)
    {
        string[] cubes = new string[] {"red", "green", "blue"};
        string[] games = line.Split(':');
        int id = int.Parse(Regex.Match(games[0], @"\d+").Value);
        // int.TryParse(games[0].First(char.IsDigit).ToString(), out int id);
        Game game = new Game(id, 0, 0, 0, false);
        string[] gamesSets = games[1].Split(';');
        foreach (string gamesSet in gamesSets)
        {
            string[] cubeSets = gamesSet.Split(',');
            foreach (string cubeSet in cubeSets)
            {
                int.TryParse(new string(cubeSet.Where(char.IsDigit).ToArray()), out int cubeCount);
                int cubeId = Array.FindIndex(cubes, cube => cubeSet.EndsWith(cube));
                switch (cubeId)
                {
                    case 0:
                        game.RedCubes = cubeCount;
                        break;
                    case 2:
                        game.BlueCubes = cubeCount;
                        break;
                    case 1:
                        game.GreenCubes = cubeCount;
                        break;
                }

                if (game.RedCubes > gameConfig.RedCubes || game.GreenCubes > gameConfig.GreenCubes ||
                    game.BlueCubes > gameConfig.BlueCubes)
                {
                    game.IsPossible = false;
                    return game;
                }
                else
                {
                    game.IsPossible = true;
                }
            }
        }

        return game;
    }

    public static int GetSumOfPower(List<string> input)
    {
        int sum = 0;
        foreach (string line in input)
        {
            Game game = ParseLineToGamePart2(line);
            sum += game.RedCubes * game.GreenCubes * game.BlueCubes;
        }

        return sum;
    }

    public static Game ParseLineToGamePart2(string line)
    {
        string[] cubes = new string[] {"red", "green", "blue"};
        string[] games = line.Split(':');
        int id = int.Parse(Regex.Match(games[0], @"\d+").Value);
        // int.TryParse(games[0].First(char.IsDigit).ToString(), out int id);
        Game game = new Game(id, 0, 0, 0, false);
        string[] gamesSets = games[1].Split(';');
        foreach (string gamesSet in gamesSets)
        {
            string[] cubeSets = gamesSet.Split(',');
            foreach (string cubeSet in cubeSets)
            {
                int.TryParse(new string(cubeSet.Where(char.IsDigit).ToArray()), out int cubeCount);
                int cubeId = Array.FindIndex(cubes, cube => cubeSet.EndsWith(cube));
                switch (cubeId)
                {
                    case 0:
                        if (cubeCount > game.RedCubes)
                        {
                            game.RedCubes = cubeCount;
                        }
                        break;
                    case 2:
                        if (cubeCount > game.BlueCubes)
                        {
                            game.BlueCubes = cubeCount;
                        }
                        break;
                    case 1:
                        if (cubeCount > game.GreenCubes)
                        {
                            game.GreenCubes = cubeCount;
                        }
                        break;
                }
            }
        }

        return game;
    }
}