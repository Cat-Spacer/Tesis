using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjective : MonoBehaviour, IInteract
{
    private bool catState;
    private bool hamsterState;

    void PlayerEnter()
    {
        if (catState && hamsterState) Finish();
    }

    void Finish()
    {
        
    }
    

    public void Interact(params object[] param)
    {
        var thisObject = (GameObject)param[0];
        var player = thisObject.GetComponent<PlayerCharacterMultiplayer>().GetCharType();
        switch (player)
        {
            case CharacterType.Cat:
                if (!catState)
                {
                    catState = true;
                    PlayerEnter();
                }
                else
                {
                    catState = false;
                }
                break;
            case CharacterType.Hamster:
                if (!hamsterState)
                {
                    hamsterState = true;
                    PlayerEnter();
                }
                else
                {
                    hamsterState = false;
                }
                break;
            case CharacterType.Null:
                break;
        }
    }

    public void ShowInteract(bool showInteractState)
    {
        
    }
}
