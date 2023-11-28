using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance;
    public CatSpecial cat;

    private void Start()
    {
        if (instance == null)
             instance = this;
    }

    public void PowerUp(SpecialMushroom_SO type)
    {
        switch (type.type)
        {
            case MushroomType.Spit:
            {
                cat.SpitMushroom(type.time);
            }
                break;
            case MushroomType.Throw:
            {
                cat.ThrowMushroom(type.time);
            }
                break;
        }
    }
}
