using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using TMPro;

public class InputDecoder : MonoBehaviour
{
    public static List<Character> characterList = new List<Character>();

    public static void PraseInputLine(string stringToPrase)
    {
        stringToPrase.Replace("\t", "");

        if (stringToPrase.StartsWith("\""))
        {
            Say(stringToPrase);
        }

        string[] separatingString = { " ", "'", "\"", "(", ")" };
        string[] args = stringToPrase.Split(separatingString, StringSplitOptions.RemoveEmptyEntries);

        foreach (var character in characterList)
        {
            if (args[0] == character.charName)
            {
                SplitToSay(stringToPrase, character);
            }
        }
    }

    #region Say Thinks

    private static void SplitToSay(string stringToPrase, Character character)
    {
        int toQuote = stringToPrase.IndexOf("\""), endQuote = stringToPrase.Length - 1;
        string stringToOutput = stringToPrase.Substring(toQuote, endQuote - toQuote);
        Say(character.charName, stringToOutput);
    }

    public static void Say(string what)
    {
        if (what == null) return;
    }
    public static void Say(string who, string what)
    {
        if (who == null) return;
    }

    #endregion
}
