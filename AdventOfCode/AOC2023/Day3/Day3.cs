using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Utilities;

namespace AOC2023.Day3;

public class Day3
{
    public static void Run()
    {
        string path = FileUtilities.GetProjectSolutionPath();
        path += @"\AOC2023\Day3\input";
        List<string> input = FileUtilities.GetListFromFile(path);

        int sum = GetSumOfAllPartNumbers(input);
        Console.WriteLine("Sum of all part numbers: " + sum);
        int sum2 = GetSumOfGears(input);
        Console.WriteLine("Sum of all gears: " + sum2);
    }

    public struct SchematicNumber
    {
        public int Code { get; set; }
        public List<char> SurroundingSymbols { get; set; }
        public int CodeLength { get; set; }
        public bool IsPartNumber { get; set; }

        public SchematicNumber(int code, int codeLength)
        {
            Code = code;
            CodeLength = codeLength;
            SurroundingSymbols = new List<char>();
            IsPartNumber = false;
        }
    }

    static int GetSumOfAllPartNumbers(List<string> input)
    {
        int sum = 0;
        for (int i = 0; i < input.Count; i++)
        {
            List<string> schematicLines = new List<string>();
            int numberIndex = -1;
            if (i == 0)
            {
                schematicLines.Add(input[i]);
                schematicLines.Add(input[i + 1]);
                numberIndex = 0;
            }
            else if (i == (input.Count - 1))
            {
                schematicLines.Add(input[i - 1]);
                schematicLines.Add(input[i]);
                numberIndex = 1;
            }
            else
            {
                schematicLines.Add(input[i - 1]);
                schematicLines.Add(input[i]);
                schematicLines.Add(input[i + 1]);
                numberIndex = 1;
            }

            List<SchematicNumber> schematicNumbers = GetSchematicNumbersFromLines(schematicLines, numberIndex);
            foreach (SchematicNumber schematicNumber in schematicNumbers)
            {
                if (schematicNumber.IsPartNumber)
                {
                    sum += schematicNumber.Code;
                }
            }
        }

        return sum;
    }

    public static List<SchematicNumber> GetSchematicNumbersFromLines(List<string> lines, int numberIndex)
    {
        List<SchematicNumber> schematicNumbers = new List<SchematicNumber>();
        string evaluatingLine = lines[numberIndex];
        Match matchPattern = Regex.Match(evaluatingLine, @"\d+");
        while (matchPattern.Success)
        {
            string code = matchPattern.Value;
            int codeLength = code.Length;
            // Console.Write("Code: " + code);
            // Console.Write("; Code length: " + codeLength);
            SchematicNumber schematicNumber = new SchematicNumber(int.Parse(code), codeLength);
            int codeIndex = matchPattern.Index;
            if (codeIndex != 0)
            {
                char symbol = evaluatingLine[codeIndex - 1];
                schematicNumber.SurroundingSymbols.Add(symbol);
                schematicNumber.IsPartNumber =
                    schematicNumber.IsPartNumber || (!char.IsDigit(symbol) && !symbol.Equals('.'));
            }

            if (codeIndex + codeLength != evaluatingLine.Length)
            {
                char symbol = evaluatingLine[codeIndex + codeLength];
                schematicNumber.SurroundingSymbols.Add(symbol);
                schematicNumber.IsPartNumber =
                    schematicNumber.IsPartNumber || (!char.IsDigit(symbol) && !symbol.Equals('.'));
            }

            if (lines.Count == 2)
            {
                string surroundingLine = numberIndex == 0 ? lines[1] : lines[0];
                int len = (codeIndex == 0 || codeIndex + codeLength == evaluatingLine.Length)
                    ? codeLength + 1
                    : codeLength + 2;
                int startingIndex = codeIndex == 0 ? 0 : codeIndex - 1;
                for (int i = startingIndex; i < startingIndex + len; i++)
                {
                    char symbol = surroundingLine[i];
                    schematicNumber.SurroundingSymbols.Add(symbol);
                    schematicNumber.IsPartNumber =
                        schematicNumber.IsPartNumber || (!char.IsDigit(symbol) && !symbol.Equals('.'));
                }
            }
            else
            {
                int len = (codeIndex == 0 || codeIndex + codeLength == evaluatingLine.Length)
                    ? codeLength + 1
                    : codeLength + 2;
                for (int i = 0; i < 2; i++)
                {
                    string surroundingLine = i == 0 ? lines[0] : lines[2];
                    int startingIndex = codeIndex == 0 ? 0 : codeIndex - 1;
                    for (int j = startingIndex; j < startingIndex + len; j++)
                    {
                        char symbol = surroundingLine[j];
                        schematicNumber.SurroundingSymbols.Add(symbol);
                        schematicNumber.IsPartNumber = schematicNumber.IsPartNumber ||
                                                       (!char.IsDigit(symbol) && !symbol.Equals('.'));
                    }
                }
            }

            // Console.Write("; Symbols: ");
            // foreach (char symbols in schematicNumber.SurroundingSymbols)
            // {
            //     Console.Write(symbols);
            // }

            // Console.Write("; Is part number? " + (schematicNumber.IsPartNumber ? "true" : "false"));

            // Console.WriteLine();

            schematicNumbers.Add(schematicNumber);
            matchPattern = matchPattern.NextMatch();
        }

        return schematicNumbers;
    }

    public struct Coords
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coords(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public struct SurroundingSymbol
    {
        public char Symbol { get; set; }
        public Coords SymbolCoords { get; set; }

        public SurroundingSymbol(char symbol, Coords coords)
        {
            Symbol = symbol;
            SymbolCoords = coords;
        }
    }

    public struct Gear
    {
        public int Code { get; set; }
        public List<SurroundingSymbol> SurroundingSymbols { get; set; }
        public int CodeLength { get; set; }

        public Gear(int code, int codeLength)
        {
            Code = code;
            CodeLength = codeLength;
            SurroundingSymbols = new List<SurroundingSymbol>();
        }
    }

    public static int GetSumOfGears(List<string> input)
    {
        List<Coords> starCoordsList = GetAllStarLocations(input);
        List<Gear> gears = GetGears(input);
        int sum = 0;
        foreach (Coords starCoords in starCoordsList)
        {
            List<Gear> gearPair = new List<Gear>();
            foreach (Gear gear in gears)
            {
                foreach (SurroundingSymbol surroundingSymbol in gear.SurroundingSymbols)
                {
                    if (surroundingSymbol.SymbolCoords.X == starCoords.X &&
                        surroundingSymbol.SymbolCoords.Y == starCoords.Y && surroundingSymbol.Symbol.Equals('*'))
                    {
                        gearPair.Add(gear);
                    }
                }
            }

            int gearRatio = gearPair.Count == 2 ? gearPair[0].Code * gearPair[1].Code : 0;
            sum += gearRatio;
        }

        return sum;
    }

    public static List<Coords> GetAllStarLocations(List<string> input)
    {
        List<Coords> starCoordsList = new List<Coords>();
        for (int y = 0; y < input.Count; y++)
        {
            string line = input[y];
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x].Equals('*'))
                {
                    Coords starCoords = new Coords(x, y);
                    starCoordsList.Add(starCoords);
                }
            }
        }

        return starCoordsList;
    }

    public static List<Gear> GetGears(List<string> input)
    {
        List<Gear> gears = new List<Gear>();
        for (int i = 0; i < input.Count; i++)
        {
            List<string> schematicLines = new List<string>();
            int numberIndex = -1;
            if (i == 0)
            {
                schematicLines.Add(input[i]);
                schematicLines.Add(input[i + 1]);
                numberIndex = 0;
            }
            else if (i == (input.Count - 1))
            {
                schematicLines.Add(input[i - 1]);
                schematicLines.Add(input[i]);
                numberIndex = 1;
            }
            else
            {
                schematicLines.Add(input[i - 1]);
                schematicLines.Add(input[i]);
                schematicLines.Add(input[i + 1]);
                numberIndex = 1;
            }

            foreach (Gear gear in GetGearsFromLines(schematicLines, numberIndex, i))
            {
                gears.Add(gear);
            }
        }

        return gears;
    }

    public static List<Gear> GetGearsFromLines(List<string> lines, int numberIndex, int lineIndex)
    {
        List<Gear> gears = new List<Gear>();
        string evaluatingLine = lines[numberIndex];
        Match matchPattern = Regex.Match(evaluatingLine, @"\d+");
        while (matchPattern.Success)
        {
            string code = matchPattern.Value;
            int codeLength = code.Length;
            // Console.Write("Code: " + code);
            // Console.Write("; Code length: " + codeLength);
            Gear gear = new Gear(int.Parse(code), codeLength);
            int codeIndex = matchPattern.Index;
            bool isGear = false;
            if (codeIndex != 0)
            {
                char symbol = evaluatingLine[codeIndex - 1];
                Coords coords = new Coords(codeIndex - 1, lineIndex);
                SurroundingSymbol surroundingSymbol = new SurroundingSymbol(symbol, coords);
                gear.SurroundingSymbols.Add(surroundingSymbol);
                isGear = isGear || (symbol.Equals('*'));
            }

            if (codeIndex + codeLength != evaluatingLine.Length)
            {
                char symbol = evaluatingLine[codeIndex + codeLength];
                Coords coords = new Coords(codeIndex + codeLength, lineIndex);
                SurroundingSymbol surroundingSymbol = new SurroundingSymbol(symbol, coords);
                gear.SurroundingSymbols.Add(surroundingSymbol);
                isGear = isGear || (symbol.Equals('*'));
            }

            if (lines.Count == 2)
            {
                string surroundingLine = numberIndex == 0 ? lines[1] : lines[0];
                int y = numberIndex == 0 ? lineIndex + 1 : lineIndex - 1;
                int len = (codeIndex == 0 || codeIndex + codeLength == evaluatingLine.Length)
                    ? codeLength + 1
                    : codeLength + 2;
                int startingIndex = codeIndex == 0 ? 0 : codeIndex - 1;
                for (int i = startingIndex; i < startingIndex + len; i++)
                {
                    char symbol = surroundingLine[i];
                    Coords coords = new Coords(i, y);
                    SurroundingSymbol surroundingSymbol = new SurroundingSymbol(symbol, coords);
                    gear.SurroundingSymbols.Add(surroundingSymbol);
                    isGear = isGear || (symbol.Equals('*'));
                }
            }
            else
            {
                int len = (codeIndex == 0 || codeIndex + codeLength == evaluatingLine.Length)
                    ? codeLength + 1
                    : codeLength + 2;
                for (int i = 0; i < 2; i++)
                {
                    string surroundingLine = i == 0 ? lines[0] : lines[2];
                    int startingIndex = codeIndex == 0 ? 0 : codeIndex - 1;
                    int y = i == 0 ? lineIndex - 1 : lineIndex + 1;
                    for (int j = startingIndex; j < startingIndex + len; j++)
                    {
                        char symbol = surroundingLine[j];
                        Coords coords = new Coords(j, y);
                        SurroundingSymbol surroundingSymbol = new SurroundingSymbol(symbol, coords);
                        gear.SurroundingSymbols.Add(surroundingSymbol);
                        isGear = isGear || (symbol.Equals('*'));
                    }
                }
            }

            // Console.Write("; Symbols: ");
            // foreach (SurroundingSymbol symbols in gear.SurroundingSymbols)
            // {
            //     Console.Write(symbols);
            // }

            // Console.Write("; Is part number? " + (schematicNumber.IsPartNumber ? "true" : "false"));

            // Console.WriteLine();

            if (isGear)
            {
                gears.Add(gear);
            }

            matchPattern = matchPattern.NextMatch();
        }

        return gears;
    }
}