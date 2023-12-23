using System.Text.RegularExpressions;
using Utilities;

namespace AOC2023.Day1;

public class Day1
{
    public static void Run()
    {
        string path = FileUtilities.GetProjectSolutionPath();
        path += @"\AOC2023\Day1\test2";
        List<string> input = FileUtilities.GetListFromFile(path);

        // int sum = GetSumOfAllCalibrationValuesFromDigits(input);
        // Console.WriteLine("Sum of all calibration values: " + sum);
        int sum = GetSumOfAllCalibrationValuesFromStringAndDigits(input);
        Console.WriteLine("Sum of all calibration values: " + sum);
    }

    static int GetSumOfAllCalibrationValuesFromDigits(List<string> input)
    {
        int sum = input.Select(GetTwoDigitNumberFromLineDigits).Sum();
        return sum;
    }

    static int GetTwoDigitNumberFromLineDigits(string line)
    {
        char firstDigit = line.First(char.IsDigit);
        char lastDigit = line.Last(char.IsDigit);
        
        string combinedDigits = $"{firstDigit}{lastDigit}";
        return int.Parse(combinedDigits);
    }

    static int GetSumOfAllCalibrationValuesFromStringAndDigits(List<string> input)
    {
        int sum = input.Select(GetTwoDigitNumberFromLineStringAndDigits).Sum();
        return sum;
    }
    
    static int GetTwoDigitNumberFromLineStringAndDigits(string line)
    {
        // string[] numbers = new string[] {"one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
        //
        // IEnumerable<(int, string)> digitIndexes = numbers.Select((number, i) =>
        //         (line.IndexOf(number, StringComparison.Ordinal), "" + (i+1)))
        //     .Where(result => result.Item1 != -1);
        //
        // int firstStringDigitIndex = digitIndexes.Select(x => x.Item1).DefaultIfEmpty(-1).Min();
        // int lastStringDigitIndex = digitIndexes.Select(x => x.Item1).DefaultIfEmpty(-1).Max();
        //
        // string firstStringDigit = digitIndexes.Select(x => x.Item2).DefaultIfEmpty("").Min();
        // string lastStringDigit = digitIndexes.Select(x => x.Item2).DefaultIfEmpty("").Max();
        //
        //
        //
        // string combinedDigits = $"{firstDigit}{lastDigit}";
        // int twoDigitNumber = int.Parse(combinedDigits);
        // return twoDigitNumber;
        Dictionary<string, int> numberMap = new Dictionary<string, int>
        {
            { "one", 1 }, { "two", 2 }, { "three", 3 }, { "four", 4 }, { "five", 5 },
            { "six", 6 }, { "seven", 7 }, { "eight", 8 }, { "nine", 9 }
        };

        List<int> digits = new List<int>();

        string[] words = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var word in words)
        {
            if (int.TryParse(word, out int number))
            {
                digits.Add(number);
            }
            else if (numberMap.TryGetValue(word, out int mappedNumber))
            {
                digits.Add(mappedNumber);
            }
        }

        if (digits.Count >= 2)
        {
            string combinedDigits = $"{digits.First()}{digits.Last()}";
            return int.Parse(combinedDigits);
        }

        return 0; // Return 0 for lines that don't have at least two valid digits/spelled-out numbers.
    }
}