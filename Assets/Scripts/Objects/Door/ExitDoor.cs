using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class ExitDoor : MonoBehaviour, IInteract
{
    Action DoorAction = delegate {  };
    private LevelObjective lvlObjective;
    private Animator anim;
    [SerializeField] private CharacterType type;
    private bool state;
    private BoxCollider2D _coll;
    private PlayerCharacter _player;
    [SerializeField] private Transform catPos;
    [SerializeField] private Transform hamsterPos;
    [SerializeField] private float lerpPos;
    private bool doorIsOpen;
    private void Start()
    {
        anim = GetComponent<Animator>();
        _coll = GetComponent<BoxCollider2D>();
        lvlObjective = GetComponentInParent<LevelObjective>();
    }

    private void Update()
    {
        DoorAction();
    }

    public void Interact(params object[] param)
    {
        if (_player == null)
        {
            var thisObject = (GameObject)param[0];
            _player = thisObject.GetComponent<PlayerCharacter>();
        }
        
        var playerType = _player.GetComponent<PlayerCharacter>().GetCharType();
        if (playerType == type)
        {
            if (!state) PlayerEnter(playerType);
            else PlayerExit();
            _coll.enabled = false;
        }
    }

    void PlayerEnter(CharacterType type)
    {
        SoundManager.instance.Play(SoundsTypes.Block, gameObject);
        anim.Play("Close_Open_Door");
        _player.EnterDoor();
        if(type == CharacterType.Cat) DoorAction += LerpCatToDoorPos;
        else DoorAction += LerpHamsterToDoorPos;
        DoorAction += PlayerCanEnter;
    }

    void LerpCatToDoorPos()
    {
        var position = _player.transform.position;
        var direction = catPos.transform.position - position;
        if (direction.magnitude < .15f)
        {
            DoorAction -= LerpCatToDoorPos;
        }
        _player.transform.position += direction * (lerpPos * Time.deltaTime);
    }
    void LerpHamsterToDoorPos()
    {
        var position = _player.transform.position;
        var direction = hamsterPos.transform.position - position;
        if (direction.magnitude < .15f)
        {
            DoorAction -= LerpHamsterToDoorPos;
        }
        _player.transform.position += direction * (lerpPos * Time.deltaTime);
    }
    void PlayerCanEnter()
    {
        if (!doorIsOpen) return;
        lvlObjective.PlayerEnter(type);
        _player.EnterDoorAnimation();
        state = true;
        StartCoroutine(WaitToClose());
        DoorAction -= PlayerCanEnter;
    }
    
    public void DoorIsOpen()
    {
        doorIsOpen = true;
    }

    public void DoorIsClosed()
    {
        doorIsOpen = false;
        _coll.enabled = true;
    }
    void PlayerExit()
    {
        SoundManager.instance.Play(SoundsTypes.Block, gameObject);
        anim.Play("Close_Open_Door");
        DoorAction += PlayerCanExit;
    }
    void PlayerCanExit()
    {
        if (!doorIsOpen) return;
        lvlObjective.PlayerExit(type);
        _player.ExitDoor();
        state = false;
        StartCoroutine(WaitToClose());
        DoorAction -= PlayerCanExit;
    }
    IEnumerator WaitToClose()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        anim.Play("Close_Close_Door");
    }
    public void ShowInteract(bool showInteractState)
    {
        
    }
}
