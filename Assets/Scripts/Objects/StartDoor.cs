using System.Collections;
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
        StartCoroutine(WaitOpenDoor());
    }

    public void CloseDoor()
    {
        StartCoroutine(WaitCloseDoor());
    }

    private IEnumerator WaitCloseDoor()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        Close();
    }
    private IEnumerator WaitOpenDoor()
    {
        yield return new WaitForEndOfFrame();
        SoundManager.instance.Play(SoundsTypes.GateOpen, gameObject);
    }
    public void Close()
    {
        _anim.Play("Start_Close_Door");
        SoundManager.instance.Play(SoundsTypes.GateClose, gameObject);
        // SoundSpawn soundSpawn = gameObject.GetComponentInChildren<SoundSpawn>();
        // if (soundSpawn)
        // {
        //     soundSpawn.UnsuscribeEventManager();
        //     if(SoundManager.instance) SoundManager.instance.RemoveFromSoundList(soundSpawn);
        //     Destroy(soundSpawn.gameObject, 1.587f);
        // }
    }

    public void None() { }
    public Transform GetPlayerPosition()
    {
        return playerPosition;
    }
    public CharacterType GetPlayerType()
    {
        return playerType;
    }
}