using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjective : MonoBehaviour
{
    private bool catState;
    private bool hamsterState;
    [SerializeField] private bool _open;

    private void Start()
    {
    }

    private void OnOpenDoors(object[] obj)
    {
        
    }

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
        EventManager.Instance.Trigger(EventType.OnFinishGame);
        StartCoroutine(Screenshot());
    }

    IEnumerator Screenshot()
    {
        yield return new WaitForSecondsRealtime(3f);
        EventManager.Instance.Trigger(EventType.ShowTv);
        StartCoroutine(FinishDelay());
    }
    IEnumerator FinishDelay()
    {
        yield return new WaitForEndOfFrame();
        GameManager.Instance.WinLevel();
    }
}
