using Utilities;

namespace AOC2022.Day4;

public class Day4
{
    public static void Run()
    {
        string path = FileUtilities.GetProjectSolutionPath();
        path += @"\AOC2022\Day4\input";
        List<string> input = FileUtilities.GetListFromFile(path);
        
        // Part 1
        int count = Part1(input);
        Console.WriteLine($"Part 1: {count}");
        
        // Part 2
        count = Part2(input);
        Console.WriteLine($"Part 2: {count}");
    }
    
    private static int Part1(List<string> input)
    {
        int count = 0;
        foreach (string pair in input)
        {
            string[] assignments = pair.Split(',');

            string[] elfAssignments1 = assignments[0].Split('-');
            IEnumerable<int> elfRange1 = Enumerable.Range(int.Parse(elfAssignments1[0]), int.Parse(elfAssignments1[1]) - int.Parse(elfAssignments1[0]) + 1);
            
            string[] elfAssignments2 = assignments[1].Split('-');
            IEnumerable<int> elfRange2 = Enumerable.Range(int.Parse(elfAssignments2[0]), int.Parse(elfAssignments2[1]) - int.Parse(elfAssignments2[0]) + 1);

            IEnumerable<int> difference1 = elfRange1.Except(elfRange2);
            IEnumerable<int> difference2 = elfRange2.Except(elfRange1);

            if (!difference1.Any() || !difference2.Any())
            {
                count++;
            }
        }
        return count;
    }
    
    private static int Part2(List<string> input)
    {
        int count = 0;
        foreach (string pair in input)
        {
            string[] assignments = pair.Split(',');

            string[] elfAssignments1 = assignments[0].Split('-');
            IEnumerable<int> elfRange1 = Enumerable.Range(int.Parse(elfAssignments1[0]), int.Parse(elfAssignments1[1]) - int.Parse(elfAssignments1[0]) + 1);
            
            string[] elfAssignments2 = assignments[1].Split('-');
            IEnumerable<int> elfRange2 = Enumerable.Range(int.Parse(elfAssignments2[0]), int.Parse(elfAssignments2[1]) - int.Parse(elfAssignments2[0]) + 1);

            IEnumerable<int> difference1 = elfRange1.Intersect(elfRange2);

            if (difference1.Any())
            {
                count++;
            }
        }
        return count;
    }
}