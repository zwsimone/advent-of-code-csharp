namespace Utilities;

public static class FileUtilities
{
    public static List<string> GetListFromFile(string path)
    {
        List<string> list = new();
        if (File.Exists(path))
        {
            list = File.ReadAllLines(path).ToList();
        }
        else
        {
            Console.WriteLine($"File not found: {path}");
        }
        
        return list;
    }
    
    public static string GetProjectSolutionPath()
    {
        string? parentDirectoryName = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
        if (parentDirectoryName != null)
        {
            DirectoryInfo programDirectoryInfo = new DirectoryInfo(parentDirectoryName);

            DirectoryInfo? parentDirectory = programDirectoryInfo.Parent; // Get the parent directory (directory "B" in your example)

            return parentDirectory?.FullName ?? string.Empty;
        }

        return string.Empty;
    }
    
    public static string GetFolderNameFromPath(string path)
    {
        string[] pathParts = path.Split(Path.DirectorySeparatorChar);
        return pathParts[^1];
    }
}