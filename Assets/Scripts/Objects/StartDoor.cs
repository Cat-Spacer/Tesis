using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDoor : MonoBehaviour
{
    [SerializeField] private CharacterType playerType;
    private Animator _anim;
    [SerializeField] private Transform playerPosition;
    private void Start()
    {
        _anim = GetComponent<Animator>();
        if (playerType == CharacterType.Cat) GameManager.Instance.SetCatRespawnPoint(transform.position);
        else GameManager.Instance.SetHamsterRespawnPoint(transform.position);
    }

    public void Open()
    {
        _anim.Play("Start_Open_Door");
        SoundManager.instance.Play(SoundsTypes.Block, gameObject);
    }

    public void CloseDoor()
    {
        StartCoroutine(WaitCloseDoor());
    }

    IEnumerator WaitCloseDoor()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        Close();
    }
    public void Close()
    {
        _anim.Play("Start_Close_Door");
        SoundManager.instance.Play(SoundsTypes.Block, gameObject);
    }
    public void None(){}
    public Transform GetPlayerPosition()
    {
        return playerPosition;
    }
    public CharacterType GetPlayerType()
    {
        return playerType;
    }
}