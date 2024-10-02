using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    public Animator _anim;
    private CharacterData _data;
    private string _currentState;
    public SpriteRenderer spRenderer;
    private Material _mat;
    
    [Header("Imgs")]
    public GameObject stunIcon;

    private void Start()
    {
        _data = GetComponent<CharacterData>();
        _mat = spRenderer.material;
    }

    public void ChangeAnimationState(string newState)
    {
        if (_currentState == newState) return;
        _anim.Play(newState);
        
        _currentState = newState;
    }
    public void FaceDirection(int direction)
    {
        //if (_data.faceDirection == direction) return;
        _data.faceDirection = direction;
        if (_data.faceDirection > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (_data.faceDirection < 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }
    public void Teletransport()
    {
        _mat.SetInteger("Tp_Bool", 1);
    }

    public void GetSmash()
    {
        ChangeAnimationState("GetSmash");
    }
    public void GetStun(bool isStun)
    {
        stunIcon.SetActive(isStun);
    }
    public float GetFaceDirection()
    {
        return _data.faceDirection;
    }
}
