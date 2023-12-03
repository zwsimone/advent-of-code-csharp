using Utilities;

namespace AOC2022.Day2;

public class Day2
{
    private enum HandShape
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    private enum Outcome
    {
        Lose = 0,
        Draw = 3,
        Win = 6
    }

    private static readonly Dictionary<char, HandShape> OpponentPlayOptions = new Dictionary<char, HandShape>
    {
        {'A', HandShape.Rock},
        {'B', HandShape.Paper},
        {'C', HandShape.Scissors}
    };

    private static readonly Dictionary<char, HandShape> PlayOptions = new Dictionary<char, HandShape>
    {
        {'X', HandShape.Rock},
        {'Y', HandShape.Paper},
        {'Z', HandShape.Scissors}
    };

    private static readonly Dictionary<char, Outcome> OutcomeOptions = new Dictionary<char, Outcome>
    {
        {'X', Outcome.Lose},
        {'Y', Outcome.Draw},
        {'Z', Outcome.Win}
    };

    public static void Run()
    {
        string path = FileUtilities.GetProjectSolutionPath();
        path += @"\AOC2022\Day2\input";
        List<string> input = FileUtilities.GetListFromFile(path);

        // Part 1
        int totalScore = GetTotalScorePart1(input);
        Console.WriteLine($"Part 1. Total score: {totalScore}");
        
        // Part 2
        totalScore = GetTotalScorePart2(input);
        Console.WriteLine($"Part 2. Total score: {totalScore}");
    }

    private static int GetTotalScorePart1(List<string> input)
    {
        int totalScore = 0;
        foreach (string round in input)
        {
            char[] handShape = round.ToCharArray();

            OpponentPlayOptions.TryGetValue(handShape[0], out HandShape opponent);
            PlayOptions.TryGetValue(handShape[2], out HandShape myself);

            if (opponent == myself) // Draw
            {
                totalScore += (int) Outcome.Draw + (int) myself;
            }
            else if ( // Win
                     (opponent == HandShape.Rock && myself == HandShape.Paper) ||
                     (opponent == HandShape.Paper && myself == HandShape.Scissors) ||
                     (opponent == HandShape.Scissors && myself == HandShape.Rock))
            {
                totalScore += (int) Outcome.Win + (int) myself;
            }
            else if ( // Lose
                     (opponent == HandShape.Rock && myself == HandShape.Scissors) ||
                     (opponent == HandShape.Paper && myself == HandShape.Rock) ||
                     (opponent == HandShape.Scissors && myself == HandShape.Paper))
            {
                totalScore += (int) Outcome.Lose + (int) myself;
            }
        }

        return totalScore;
    }

    private static int GetTotalScorePart2(List<string> input)
    {
        int totalScore = 0;
        foreach (string round in input)
        {
            char[] roundArray = round.ToCharArray();

            OpponentPlayOptions.TryGetValue(roundArray[0], out HandShape opponent);
            OutcomeOptions.TryGetValue(roundArray[2], out Outcome outcome);
            
            if (outcome == Outcome.Draw)
            {
                totalScore += (int) Outcome.Draw + (int) opponent;
            }
            else if (outcome == Outcome.Win)
            {
                int myself = opponent switch
                {
                    HandShape.Rock => (int) HandShape.Paper,
                    HandShape.Paper => (int) HandShape.Scissors,
                    HandShape.Scissors => (int) HandShape.Rock
                };
                totalScore += (int) Outcome.Win + (int) myself;
            }
            else if (outcome == Outcome.Lose)
            {
                int myself = opponent switch
                {
                    HandShape.Rock => (int) HandShape.Scissors,
                    HandShape.Paper => (int) HandShape.Rock,
                    HandShape.Scissors => (int) HandShape.Paper
                };
                totalScore += (int) Outcome.Lose + (int) myself;
            }
        }

        return totalScore;
    }
}