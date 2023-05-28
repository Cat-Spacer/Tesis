using UnityEngine;

public class Character
{
    public string charName;
    public Color charColor = Color.white;
    public string sideImage = null;

    public Character(string nameInput, Color coloInput, string sideImageInput)
    {
        charName = nameInput;
        charColor = coloInput;
        if (sideImageInput != null)
            sideImage = sideImageInput;
    }

    public Character(string nameInput)
    {
        charName = nameInput;
        charColor = Color.white;
        sideImage = null;
    }

    public Character(string nameInput, Color coloInput)
    {
        charName = nameInput;
        charColor = coloInput;
        sideImage = null;
    }
}
