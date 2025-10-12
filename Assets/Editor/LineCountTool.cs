#if UNITY_EDITOR
using UnityEngine;
using System.IO;
using UnityEditor;
public class LineCountTool
{
    [MenuItem("Tools/Count Lines of Code")]
    public static void CountLines()
    {
        string[] files = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
        int totalLines = 0;

        foreach (var file in files)
        {
            string[] lines = File.ReadAllLines(file);
            totalLines += lines.Length;
        }

        Debug.Log($"Total C# lines of code: {totalLines}");
    }
}
#endif