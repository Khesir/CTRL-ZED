using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamUtils : MonoBehaviour
{
    public static string GenerateTeamID()
    {
        // Example format: TEAM-20250703-AX4K7 (DATE + 5-char random alphanumeric)
        string datePart = DateTime.Now.ToString("yyyyMMdd");
        string randomPart = GenerateRandomString(5);
        return $"TEAM-{datePart}-{randomPart}";
    }

    private static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        System.Random random = new System.Random();
        char[] buffer = new char[length];
        for (int i = 0; i < length; i++)
        {
            buffer[i] = chars[random.Next(chars.Length)];
        }
        return new string(buffer);
    }
}
