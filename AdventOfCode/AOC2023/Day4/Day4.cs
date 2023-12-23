using Utilities;

namespace AOC2023.Day4;

public class Day4
{
    public static void Run()
    {
        string path = FileUtilities.GetProjectSolutionPath();
        path += @"\AOC2023\Day4\input";
        List<string> input = FileUtilities.GetListFromFile(path);

        int points = GetTotalPoints(input);
        Console.WriteLine("Total points: " + points);
        int totalCards = GetTotalScratchCards(input);
        Console.WriteLine("Total scratch cards: " + totalCards);
    }

    public static int GetTotalPoints(List<string> input)
    {
        int points = 0;
        foreach (string line in input)
        {
            string[] card = line.Split(':');
            string[] numbers = card[1].Split('|');
            string[] winningNumbers =
                numbers[0].Trim().Split(' ').Where(number => !string.IsNullOrEmpty(number)).ToArray();
            string[] numberOnHand =
                numbers[1].Trim().Split(' ').Where(number => !string.IsNullOrEmpty(number)).ToArray();

            int winningNumberCount = 0;

            foreach (string number in numberOnHand)
            {
                if (winningNumbers.Contains(number))
                {
                    winningNumberCount++;
                }
            }

            points += winningNumberCount > 0 ? (int) Math.Pow(2, (winningNumberCount - 1)) : 0;
        }

        return points;
    }

    public static int GetTotalScratchCards(List<string> input)
    {
        int[] cardInstanceCount = new int[input.Count];
        for (int y = 0; y < input.Count; y++)
        {
            cardInstanceCount[y] = 1;
        }

        for (int i = 0; i < input.Count; i++)
        {
            string line = input[i];
            string[] card = line.Split(':');
            string[] numbers = card[1].Split('|');
            string[] winningNumbers =
                numbers[0].Trim().Split(' ').Where(number => !string.IsNullOrEmpty(number)).ToArray();
            string[] numberOnHand =
                numbers[1].Trim().Split(' ').Where(number => !string.IsNullOrEmpty(number)).ToArray();

            int winningNumberCount = 0;

            foreach (string number in numberOnHand)
            {
                if (winningNumbers.Contains(number))
                {
                    winningNumberCount++;
                }
            }

            for (int z = 0; z < cardInstanceCount[i]; z++)
            {
                for (int x = i; x < i + winningNumberCount; x++)
                {
                    cardInstanceCount[x + 1]++;
                }
            }
        }

        return cardInstanceCount.Sum();
    }
}