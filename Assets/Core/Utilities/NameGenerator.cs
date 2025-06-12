using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NameGenerator
{
    private static readonly string[] firstNames = { "Nova", "Zane", "Luna", "Astra", "Echo", "Juno" };
    private static readonly string[] lastNames = { "Storm", "Shadow", "Vega", "Ray", "Blaze", "Drift" };

    public static string GetRandomName()
    {
        string first = firstNames[Random.Range(0, firstNames.Length)];
        string last = lastNames[Random.Range(0, lastNames.Length)];
        return $"{first} {last}";
    }
}
