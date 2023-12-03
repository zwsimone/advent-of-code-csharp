using Utilities;

namespace AOC2022.Day3
{
    public class Day3
    {
        public static void Run()
        {
            string path = FileUtilities.GetProjectSolutionPath();
            path += @"\AOC2022\Day3\input";
            List<string> input = FileUtilities.GetListFromFile(path);

            // Part 1
            // List<char> sharedItems = GetSharedItems(input);

            // Part 2
            List<char> sharedBadges = GetSharedBadges(input);

            int prioritySum = GetPrioritySum(sharedBadges);
            Console.WriteLine("Total sum of priority is {0}", prioritySum);
        }

        public static List<char> GetSharedItems(List<string> input)
        {
            List<char> sharedItems = new List<char>();
            foreach (string rucksack in input)
            {
                string firstCompartment = rucksack.Substring(0, rucksack.Length / 2);
                string secondCompartment = rucksack.Substring(rucksack.Length / 2);
                for (int i = 0; i < firstCompartment.Length; i++)
                {
                    if (secondCompartment.Contains(firstCompartment[i]))
                    {
                        sharedItems.Add(firstCompartment[i]);
                        break;
                    }
                }
            }

            return sharedItems;
        }

        public static int GetPrioritySum(List<char> sharedItems)
        {
            int sum = 0;
            foreach (char item in sharedItems)
            {
                if (Char.IsUpper(item))
                {
                    sum += item - 38;
                }
                else
                {
                    sum += item - 96;
                }
            }

            return sum;
        }

        public static List<char> GetSharedBadges(List<string> input)
        {
            List<char> sharedBadges = new List<char>();
            string[] elfGroup = new string[3];
            int elfIndex = 0;
            for (int i = 0; i < input.Count; i++)
            {
                elfGroup[elfIndex] = input[i];
                elfIndex++;
                if ((i + 1) % 3 == 0)
                {
                    elfGroup = elfGroup.OrderByDescending(elf => elf.Length).ToArray();
                    string elfWithMaxItems = elfGroup[0];
                    for (int j = 0; j < elfWithMaxItems.Length; j++)
                    {
                        char badge = elfWithMaxItems[j];
                        if (elfGroup[1].Contains(badge) && elfGroup[2].Contains(badge))
                        {
                            sharedBadges.Add(badge);
                            elfGroup = new string[3];
                            elfIndex = 0;
                            break;
                        }
                    }
                }
            }

            return sharedBadges;
        }
    }
}