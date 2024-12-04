using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class RecursiveFileProcessor
{
    [Serializable]
    public class SortHelper : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x.Length.CompareTo(y.Length) != 0) return x.Length.CompareTo(y.Length);
            else return string.Compare(x, y, StringComparison.Ordinal);
        }
    }

    public static void Main(string[] args)
    {
        foreach (string path in args)
        {
            if (Directory.Exists(path))
            {
                SortedDictionary<string, long> elements = new SortedDictionary<string, long>(new SortHelper());
                ProcessDirectory(new DirectoryInfo(path), 0, elements);
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                Console.WriteLine("\nOldest file: {0}\n", directoryInfo.OldestFileCreationDate());
                foreach (KeyValuePair<string, long> element in elements)
                {
                    Console.WriteLine("{0} -> {1}", element.Key, element.Value);
                }

                Serialize(elements, "elements.bin");

                SortedDictionary<string, long> deserializedElements = Deserialize<SortedDictionary<string, long>>("elements.bin");
                Console.WriteLine("\nDeserialized elements:");
                foreach (KeyValuePair<string, long> element in deserializedElements)
                {
                    Console.WriteLine("{0} -> {1}", element.Key, element.Value);
                }
            }
            else
            {
                Console.WriteLine("{0} is not a valid file or directory.", path);
            }
        }
    }

    public static void Serialize<T>(T obj, string filename)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(filename, FileMode.Create))
        {
            formatter.Serialize(stream, obj);
        }
    }

    public static T Deserialize<T>(string filename)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(filename, FileMode.Open))
        {
            return (T)formatter.Deserialize(stream);
        }
    }

    public static void ProcessFile(string filename, SortedDictionary<string, long> elements)
    {
        FileInfo fileInfo = new FileInfo(filename);
        Console.WriteLine("{0} {1} bytes {2}", fileInfo.Name, fileInfo.Length, fileInfo.DOSAttributes());
        elements.Add(fileInfo.Name, fileInfo.Length);
    }
    public static void ProcessDirectory(DirectoryInfo directory, int depth, SortedDictionary<string, long> elements)
    {
        FileInfo[] files = directory.GetFiles();
        DirectoryInfo[] subdirectories = directory.GetDirectories();
        for (int i = 0; i < depth; i++)
            Console.Write("     ");
        System.Console.WriteLine("{0} ({1}) {2}", directory.Name, directory.GetFileSystemInfos().Length, directory.DOSAttributes());
        elements.Add(directory.Name, directory.GetFileSystemInfos().Length);
        foreach (FileInfo file in files)
        {
            for (int i = 0; i < depth; i++)
                Console.Write("     ");
            ProcessFile(file.FullName, elements);
        }

        foreach (DirectoryInfo subdir in subdirectories)
        {
            ProcessDirectory(subdir, depth + 1, elements);
        }
    }

    public static DateTime OldestFileCreationDate(this DirectoryInfo dir)
    {
        DateTime oldestDate = DateTime.MaxValue;

        foreach (FileInfo file in dir.EnumerateFiles())
            if (file.CreationTime < oldestDate) oldestDate = file.CreationTime;

        foreach (DirectoryInfo subdir in dir.EnumerateDirectories())
        {
            DateTime subOldestDate = subdir.OldestFileCreationDate();
            if (subOldestDate < oldestDate) oldestDate = subOldestDate;
        }

        return oldestDate;
    }

    public static String DOSAttributes(this FileSystemInfo fileInfo)
    {
        string attributes = "";

        if ((fileInfo.Attributes & FileAttributes.ReadOnly) != 0)
            attributes += "r";
        else
            attributes += "-";

        if ((fileInfo.Attributes & FileAttributes.Archive) != 0)
            attributes += "a";
        else
            attributes += "-";

        if ((fileInfo.Attributes & FileAttributes.Hidden) != 0)
            attributes += "h";
        else
            attributes += "-";

        if ((fileInfo.Attributes & FileAttributes.System) != 0)
            attributes += "s";
        else
            attributes += "-";

        return attributes;
    }
}
