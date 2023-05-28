using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;

public class InputDecoder : MonoBehaviour
{
    //[SerializeField] private string loadPath = "Prefabs/Novel", findUI = "Novel_UI", findCanvas = "Novel_Canvas";
    //instanciar todos los find

    public static List<Label> labels = new List<Label>();
    public static List<Character> characterList = new List<Character>();
    public static bool pause = false;


    private static GameObject canvas = GameObject.Find("Novel_Canvas");
    private static GameObject interfaceElements = GameObject.Find("Novel_UI");
    public static GameObject dialogTextObject = GameObject.Find("Dialog_Text");
    public static GameObject namePlateTextObject = GameObject.Find("NamePlate_Text");
    private static GameObject imageInst = Resources.Load("Prefabs/Novel") as GameObject;

    [NonSerialized]
    public static List<string> commands = new List<string>();

    public string inputLine;
    public static int commandLine = 0;
    public static string lastCommand = string.Empty;

    /*private void Start()
    {
        if (imageInst == null && Resources.Load(loadPath))
            imageInst = Resources.Load(loadPath) as GameObject;
    }*/

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

        if (args[0] == "Character")
        {
            CreateNewCharacter(stringToPrase);
        }

        if (args[0] == "jump")
        {
            jumpTo(stringToPrase);
        }
    }

    #region New Character

    private static void CreateNewCharacter(string stringToPrase)
    {
        string newCharName = null;
        Color newColor = Color.white;
        string newImage = null;

        var CharacterExpression = new Regex(@"Character\((?<charName>[a-zA-Z0-9_]+), color=(?<charColor>[a-zA-Z0-9_]), image=(?<sideImage>[a-zA-Z0-9_])\)");

        if (CharacterExpression.IsMatch(stringToPrase))
        {
            var matches = CharacterExpression.Match(stringToPrase);
            newCharName = matches.Groups["charName"].ToString();
            newColor = Color.clear;
            ColorUtility.TryParseHtmlString(matches.Groups["charColor"].ToString(), out newColor);
            newImage = matches.Groups["sideImage"].ToString();
        }

        characterList.Add(new Character(newCharName, newColor, newImage));
    }

    #endregion

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
        if (!interfaceElements.activeInHierarchy) interfaceElements.SetActive(true);
        dialogTextObject.GetComponent<TextMeshProUGUI>().text = what;
        pause = true;
    }
    public static void Say(string who, string what)
    {
        if (who == null) return;
        if (!interfaceElements.activeInHierarchy) interfaceElements.SetActive(true);
        dialogTextObject.GetComponent<TextMeshProUGUI>().text = what;
        namePlateTextObject.GetComponent<TextMeshProUGUI>().text = who;
        pause = true;
    }

    #endregion

    #region LoadScript

    public static void readScript()
    {
        for (int i = 0; i < commands.Count; i++)
        {
            if (commands[i].StartsWith("label"))
            {
                var labelSplit = commands[i].Split(' ');
                labels.Add(new Label(labelSplit[1], i));

            }
        }
    }

    #endregion

    public static void jumpTo(string stringToPrase)
    {
        var tempStringSplit = stringToPrase.Split(' ');

        foreach (var l in labels)
        {
            if (l.labelName == tempStringSplit[1])
            {
                commandLine = l.labelIndex;
                pause = false;
            }
        }
    }
}
