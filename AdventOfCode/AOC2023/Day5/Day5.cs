using Utilities;

namespace AOC2023.Day5;

public class Day5
{
    public static void Run()
    {
        string path = FileUtilities.GetProjectSolutionPath();
        path += @"\AOC2023\Day5\test1";
        List<string> input = FileUtilities.GetListFromFile(path);

        // ulong lowestLocationNumber = GetLowestLocationNumberByValue(input);
        // Console.WriteLine("Lowest location number: " + lowestLocationNumber);
        ulong lowestLocationNumbers = GetLowestLocationNumbersByRange(input);
        Console.WriteLine("Lowest location number: " + lowestLocationNumbers);
    }

    public struct CategoryMap
    {
        public ulong DestinationRangeStart { get; set; }
        public ulong SourceRangeStart { get; set; }
        public ulong RangeLength { get; set; }

        public CategoryMap(ulong destinationRangeStart, ulong sourceRangeStart, ulong rangeLength)
        {
            DestinationRangeStart = destinationRangeStart;
            SourceRangeStart = sourceRangeStart;
            RangeLength = rangeLength;
        }
    }

    public static ulong GetLowestLocationNumberByValue(List<string> input)
    {
        GetSeedsAndCategoryMap(input, out var seeds, out var categoryMap);

        ulong lowestLocationNumber = SeedToLocationByValue(seeds, categoryMap);

        return lowestLocationNumber;
    }

    public static ulong GetLowestLocationNumbersByRange(List<string> input)
    {
        GetSeedsAndCategoryMap(input, out var seeds, out var categoryMap);

        ulong lowestLocationNumber = SeedToLocationByRange(seeds, categoryMap);

        return lowestLocationNumber;
    }

    public static void GetSeedsAndCategoryMap(List<string> input, out ulong[] seeds,
        out Dictionary<string, List<CategoryMap>> categoryMap)
    {
        seeds = new ulong[input[0].Split(' ').Length - 1];
        categoryMap = new Dictionary<string, List<CategoryMap>>();
        List<string> category = new List<string>();
        for (int i = 0; i < input.Count; i++)
        {
            string line = input[i];
            if (!string.IsNullOrEmpty(line))
            {
                category.Add(line);
            }

            if (string.IsNullOrEmpty(line) || i == input.Count - 1)
            {
                if (category[0].Contains("seed-to-soil map"))
                {
                    categoryMap.Add("seed-to-soil", GetMap(category));
                }
                else if (category[0].Contains("soil-to-fertilizer map"))
                {
                    categoryMap.Add("soil-to-fertilizer", GetMap(category));
                }
                else if (category[0].Contains("fertilizer-to-water map"))
                {
                    categoryMap.Add("fertilizer-to-water", GetMap(category));
                }
                else if (category[0].Contains("water-to-light map"))
                {
                    categoryMap.Add("water-to-light", GetMap(category));
                }
                else if (category[0].Contains("light-to-temperature map"))
                {
                    categoryMap.Add("light-to-temperature", GetMap(category));
                }
                else if (category[0].Contains("temperature-to-humidity map"))
                {
                    categoryMap.Add("temperature-to-humidity", GetMap(category));
                }
                else if (category[0].Contains("humidity-to-location map"))
                {
                    categoryMap.Add("humidity-to-location", GetMap(category));
                }
                else if (category[0].Contains("seed"))
                {
                    string[] arr = category[0].Split(':');
                    seeds = arr[1].Trim().Split(' ').Select(number => ulong.Parse(number)).ToArray();
                }

                category.Clear();
            }
        }
    }

    public static List<CategoryMap> GetMap(List<string> category)
    {
        List<CategoryMap> categoryMap = new List<CategoryMap>();
        foreach (string line in category)
        {
            if (!line.Contains("map"))
            {
                ulong[] map = line.Split(' ').Select(number => ulong.Parse(number)).ToArray();
                categoryMap.Add(new CategoryMap(map[0], map[1], map[2]));
            }
        }

        return categoryMap;
    }

    public static ulong SeedToLocationByValue(ulong[] seeds, Dictionary<string, List<CategoryMap>> categoryMap)
    {
        ulong lowestLocationNumber = ulong.MaxValue;
        foreach (ulong seed in seeds)
        {
            ulong currentSeed = seed;
            // Console.Write(seed + "; ");
            foreach (var mapType in categoryMap)
            {
                // mapType.Value.Sort((x, y) => x.SourceRangeStart.CompareTo(y.SourceRangeStart));
                IsInMap(mapType, currentSeed, out currentSeed);

                // Console.Write(mapType.Key + " " + currentSeed + "; ");
                if (mapType.Key.Equals("humidity-to-location") && currentSeed < lowestLocationNumber)
                {
                    lowestLocationNumber = currentSeed;
                }
            }

            // Console.WriteLine();
        }

        return lowestLocationNumber;
    }

    public static void IsInMap(KeyValuePair<string, List<CategoryMap>> mapType, ulong currentSeed, out ulong newSeed)
    {
        foreach (var map in mapType.Value)
        {
            ulong sourceRangeEnd = map.SourceRangeStart + map.RangeLength;
            if (currentSeed >= map.SourceRangeStart && currentSeed < sourceRangeEnd)
            {
                newSeed = map.DestinationRangeStart + (currentSeed - map.SourceRangeStart);
                return;
            }
        }

        newSeed = currentSeed;
    }

    public struct RangeInMap
    {
        public ulong RangeSrcStart { get; set; }
        public ulong RangeDelta { get; set; }

        public RangeInMap(ulong rangeSrcStart, ulong rangeDelta)
        {
            RangeSrcStart = rangeSrcStart;
            RangeDelta = rangeDelta;
        }
    }

    public static ulong SeedToLocationByRange(ulong[] seeds, Dictionary<string, List<CategoryMap>> categoryMap)
    {
        ulong lowestLocationNumber = ulong.MaxValue;
        for (int i = 0; i < seeds.Length; i += 2)
        {
            ulong seedStart = seeds[i];
            ulong seedRange = seeds[i + 1];
            ulong seedEnd = seedStart + seedRange;

            List<RangeInMap> currentRangesInMap = new List<RangeInMap>();
            currentRangesInMap.Add(new RangeInMap(seedStart, seedRange));
            List<RangeInMap> newRangesInMap = new List<RangeInMap>();
            foreach (var mapType in categoryMap)
            {
                mapType.Value.Sort((x, y) => x.SourceRangeStart.CompareTo(y.SourceRangeStart));
                foreach (RangeInMap range in currentRangesInMap)
                {
                    ulong currentSeedStart = range.RangeSrcStart;
                    ulong currentSeedEnd = range.RangeSrcStart + range.RangeDelta;
                    foreach (var map in mapType.Value)
                    {
                        // beginning o f  range
                        if (currentSeedStart >= map.SourceRangeStart &&
                            currentSeedStart < map.SourceRangeStart + map.RangeLength &&
                            currentSeedEnd < map.SourceRangeStart + map.RangeLength)
                        {
                            ulong destStart = map.DestinationRangeStart + (currentSeedStart - map.SourceRangeStart);
                            ulong delta = currentSeedEnd - currentSeedStart;
                            newRangesInMap.Add(new RangeInMap(destStart, delta));
                        }
                        // end of range
                        else if (currentSeedStart >= map.SourceRangeStart &&
                                 currentSeedStart < map.SourceRangeStart + map.RangeLength &&
                                 currentSeedEnd >= map.SourceRangeStart + map.RangeLength)
                        {
                            ulong destStart = map.DestinationRangeStart + (currentSeedStart - map.SourceRangeStart);
                            // ulong delta = Math.Min(map.RangeLength - (currentSeedStart - map.SourceRangeStart), currentSeedEnd - currentSeedStart);
                            ulong delta = map.SourceRangeStart + map.RangeLength - currentSeedStart;
                            newRangesInMap.Add(new RangeInMap(destStart, delta));
                        }
                        // middle of range
                        else if (currentSeedStart < map.SourceRangeStart &&
                                 currentSeedEnd > map.SourceRangeStart + map.RangeLength)
                        {
                            ulong destStart = map.DestinationRangeStart;
                            // ulong delta = Math.Min(map.RangeLength, currentSeedEnd - map.SourceRangeStart);
                            ulong delta = map.RangeLength;
                            newRangesInMap.Add(new RangeInMap(destStart, delta));
                        }
                        else if (currentSeedEnd > map.SourceRangeStart &&
                                 currentSeedEnd <= map.SourceRangeStart + map.RangeLength)
                        {
                            ulong destStart = map.DestinationRangeStart;
                            // ulong delta = Math.Min(currentSeedEnd - map.SourceRangeStart, map.RangeLength);
                            ulong delta = currentSeedEnd - map.SourceRangeStart;
                            newRangesInMap.Add(new RangeInMap(destStart, delta));
                        }
                    }
                    
                    // if empty, add full range
                    if (newRangesInMap.Count == 0)
                    {
                        ulong destStart = currentSeedStart;
                        ulong delta = currentSeedEnd - currentSeedStart;
                        newRangesInMap.Add(new RangeInMap(destStart, delta));
                    }

                    // check ends
                    if (currentSeedStart != newRangesInMap.First().RangeSrcStart)
                    {
                        ulong destStart = currentSeedStart;
                        ulong delta = newRangesInMap.First().RangeSrcStart - currentSeedStart;
                        newRangesInMap.Add(new RangeInMap(destStart, delta));
                    }

                    if (currentSeedEnd != newRangesInMap.Last().RangeSrcStart + newRangesInMap.Last().RangeDelta)
                    {
                        ulong destStart = newRangesInMap.Last().RangeSrcStart;
                        ulong delta = currentSeedEnd - newRangesInMap.Last().RangeSrcStart;
                        newRangesInMap.Add(new RangeInMap(destStart, delta));
                    }

                    newRangesInMap.Sort((x, y) => x.RangeSrcStart.CompareTo(y.RangeSrcStart));

                    currentRangesInMap = newRangesInMap;
                    newRangesInMap.Clear();
                }
            }

            lowestLocationNumber = currentRangesInMap.First().RangeSrcStart;
        }

        return lowestLocationNumber;
    }

// too memory intensive
// public static uint GetLowestLocationNumber(List<string> input)
// {
//     uint[] seeds = new uint[input[0].Split(' ').Length - 1];
//     Dictionary<string, Dictionary<uint, uint>> categoryMap = new Dictionary<string, Dictionary<uint, uint>>();
//     uint lowestLocationNumber = uint.MaxValue;
//     List<string> category = new List<string>();
//     for (int i = 0; i < input.Count; i++)
//     {
//         string line = input[i];
//         if (!string.IsNullOrEmpty(line))
//         {
//             category.Add(line);
//         }
//
//         if (string.IsNullOrEmpty(line) || i == input.Count - 1)
//         {
//             if (category[0].Contains("seed-to-soil map"))
//             {
//                 categoryMap.Add("seed-to-soil", GetMap(category));
//             }
//             else if (category[0].Contains("soil-to-fertilizer map"))
//             {
//                 categoryMap.Add("soil-to-fertilizer", GetMap(category));
//             }
//             else if (category[0].Contains("fertilizer-to-water map"))
//             {
//                 categoryMap.Add("fertilizer-to-water", GetMap(category));
//             }
//             else if (category[0].Contains("water-to-light map"))
//             {
//                 categoryMap.Add("water-to-light", GetMap(category));
//             }
//             else if (category[0].Contains("light-to-temperature map"))
//             {
//                 categoryMap.Add("light-to-temperature", GetMap(category));
//             }
//             else if (category[0].Contains("temperature-to-humidity map"))
//             {
//                 categoryMap.Add("temperature-to-humidity", GetMap(category));
//             }
//             else if (category[0].Contains("humidity-to-location map"))
//             {
//                 categoryMap.Add("humidity-to-location", GetMap(category));
//             }
//             else if (category[0].Contains("seed"))
//             {
//                 string[] arr = category[0].Split(':');
//                 seeds = arr[1].Trim().Split(' ').Select(number => uint.Parse(number)).ToArray();
//             }
//
//             category.Clear();
//         }
//     }
//
//     foreach (uint seed in seeds)
//     {
//         uint currentSeed = seed;
//         foreach (var mapType in categoryMap)
//         {
//             foreach (var map in mapType.Value)
//             {
//                 bool next = false;
//                 if (map.Key == currentSeed)
//                 {
//                     currentSeed = map.Value;
//                     next = true;
//                 }
//
//                 if (mapType.Key.Equals("humidity-to-location") && currentSeed < lowestLocationNumber)
//                 {
//                     lowestLocationNumber = currentSeed;
//                 }
//
//                 if (next)
//                 {
//                     break;
//                 }
//             }
//
//             Console.Write(mapType.Key + " " + currentSeed + "; ");
//         }
//
//         Console.WriteLine();
//     }
//
//     return lowestLocationNumber;
// }
//
// public static Dictionary<uint, uint> GetMap(List<string> category)
// {
//     Dictionary<uint, uint> categoryMap = new Dictionary<uint, uint>();
//     foreach (string line in category)
//     {
//         if (!line.Contains("map"))
//         {
//             uint[] map = line.Split(' ').Select(number => uint.Parse(number)).ToArray();
//             for (uint i = 0; i < map[2]; i++)
//             {
//                 categoryMap.Add(map[1] + i, map[0] + i);
//             }
//         }
//     }
//
//     return categoryMap;
// }
}