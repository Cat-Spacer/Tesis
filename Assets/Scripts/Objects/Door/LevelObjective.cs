using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjective : MonoBehaviour
{
    private bool catState;
    private bool hamsterState;
   

    public void PlayerEnter(CharacterType type)
    {
        if (type == CharacterType.Cat)
        {
            catState = true;
        }
        else
        {
            hamsterState = true;
        }
        if(catState && hamsterState) Finish();
    }

    public void PlayerExit(CharacterType type)
    {
        if (type == CharacterType.Cat)
        {
            catState = false;
        }
        else
        {
            hamsterState = false;
        }
    }

    void Finish()
    {
        StartCoroutine(FinishDelay());
    }

    IEnumerator FinishDelay()
    {
        yield return new WaitForSecondsRealtime(2.4f);
        GameManager.Instance.WinLevel();
    }
}
