using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterModel : NetworkBehaviour
{
    public Animator _anim;
    private string _currentState;
    public SpriteRenderer spRenderer;

    private float currentDirection;

    [Header("Imgs")]
    public GameObject stunIcon;
    
    public void ChangeAnimationState(string newState)
    {
        if (_currentState == newState) return;
        _anim.Play(newState);
        
        _currentState = newState;
    }
    public void FaceDirection(float direction)
    {
        currentDirection = direction;
        if (direction > 0)
        {
            spRenderer.flipX = false;
            FaceDirectionRpc(false);
        }
        else if (direction < 0)
        {
            spRenderer.flipX = true;
            FaceDirectionRpc(true);
        }
    }

    [Rpc(SendTo.NotMe)]
    void FaceDirectionRpc(bool boolean)
    {
        spRenderer.flipX = boolean;
    }
    public void GetStun(bool isStun)
    {
        stunIcon.SetActive(isStun);
    }
    public float GetFaceDirection()
    {
        return currentDirection;
    }
}
