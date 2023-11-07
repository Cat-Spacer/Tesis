using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    public Animator _anim;
    private string _currentState;

    [Header("Imgs")]
    public GameObject stunIcon;
    
    public void ChangeAnimationState(string newState)
    {
        if (_currentState == newState) return;
        _anim.Play(newState);

        _currentState = newState;
    }

    public void GetStun(bool isStun)
    {
        stunIcon.SetActive(isStun);
    }
}
