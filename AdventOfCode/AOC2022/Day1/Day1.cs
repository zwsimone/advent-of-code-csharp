using Utilities;

namespace AOC2022.Day1;

public class Day1
{
    public static void Run()
    {
        string path = FileUtilities.GetProjectSolutionPath();
        path += @"\AOC2022\Day1\input";
        List<string> input = FileUtilities.GetListFromFile(path);

        // Part 1
        int elfWithMostCalories = GetElfWithMostCalories(input);
        Console.WriteLine("Part 1: Elf with most calories is {0}", elfWithMostCalories);
        
        // Part 2
        int topThreeElvesWithMostCalories = GetTopThreeElvesWithMostCalories(input);
        Console.WriteLine("Part 2: Top three elves with most calories is {0}", topThreeElvesWithMostCalories);
    }

    private static int GetElfWithMostCalories(List<string> input)
    {
        int elfWithMostCalories = 0;
        int numberOfCalories = 0;
        input.ForEach(item =>
        {
            if (item.Equals(""))
            {
                if (numberOfCalories > elfWithMostCalories)
                {
                    elfWithMostCalories = numberOfCalories;
                }

                numberOfCalories = 0;
            }
            else
            {
                numberOfCalories += int.Parse(item);
            }
        });
        return elfWithMostCalories;
    }
    
    private static int GetTopThreeElvesWithMostCalories(List<string> input)
    {
        List<int> elfCalories = new List<int>();
        int numberOfCalories = 0;

        foreach (string item in input)
        {
            if (string.IsNullOrEmpty(item))
            {
                elfCalories.Add(numberOfCalories);
                numberOfCalories = 0;
            }
            else
            {
                if (int.TryParse(item, out int result))
                {
                    numberOfCalories += result;
                }
            }
        }
        
        elfCalories.Add(numberOfCalories);
        
        return elfCalories.OrderByDescending(elfCalorie => elfCalorie).Take(3).Sum();
    }
}