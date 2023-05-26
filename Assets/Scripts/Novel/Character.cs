using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character
{
    public string charName;
    public Color color = Color.white;
    public Image sideImage = null;

    public Character(string nameInput, Color coloInput, Image sideImageInput)
    {
        charName = nameInput;
        color = coloInput;
        sideImage = sideImageInput;
    }

    public Character(string nameInput)
    {
        charName = nameInput;
        color = Color.white;
        sideImage = null;
    }

    public Character(string nameInput, Color coloInput)
    {
        charName = nameInput;
        color = coloInput;
        sideImage = null;
    }
}
