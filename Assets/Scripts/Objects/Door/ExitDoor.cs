using System;
using System.Collections;
using UnityEngine;

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
    private bool doorIsUnlocked;
    private void Start()
    {
        anim = GetComponent<Animator>();
        _coll = GetComponent<BoxCollider2D>();
        lvlObjective = GetComponentInParent<LevelObjective>();
        EventManager.Instance.Subscribe(EventType.OnOpenDoors, OnOpenDoors);
    }

    private void OnOpenDoors(object[] obj)
    {
        doorIsUnlocked = true;
        OpenDoor();
    }

    private void Update()
    {
        DoorAction();
    }

    public void Interact(params object[] param)
    {
        if (!doorIsUnlocked) return;
        if (!_player)
        {
            var thisObject = (GameObject)param[0];
            _player = thisObject.GetComponent<PlayerCharacter>();
        }
        
        var playerType = _player.GetComponent<PlayerCharacter>().GetCharType();
        if (playerType != type) return;
        if (!state) PlayerEnter(playerType);
        else PlayerExit();
        _coll.enabled = false;
    }

    private void OpenDoor()
    {
        if (!doorIsUnlocked) return;
        anim.Play("Exit_Open_Door");
    }

    private void PlayerEnter(CharacterType type)
    {
        SoundManager.instance.Play(SoundsTypes.GateClose, gameObject);
        lvlObjective.PlayerEnter(type);
        _player.EnterDoor();
        if(type == CharacterType.Cat) DoorAction += LerpCatToDoorPos;
        else DoorAction += LerpHamsterToDoorPos;
        DoorAction += PlayerCanEnter;
    }

    private void LerpCatToDoorPos()
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

    private void PlayerCanEnter()
    {
        if (!doorIsOpen) return;
        _player.EnterDoorAnimation();
        state = true;
        StartCoroutine(WaitToClose());
        DoorAction -= PlayerCanEnter;
    }
    
    public void DoorIsOpen()
    {
        doorIsOpen = true;
        _coll.enabled = true;
    }

    public void DoorIsClosed()
    {
        doorIsOpen = false;
        _coll.enabled = true;
    }

    private void PlayerExit()
    {
        SoundManager.instance.Play(SoundsTypes.GateOpen, gameObject);
        anim.Play("Exit_Open_Door");
        DoorAction = PlayerCanExit;
    }

    private void PlayerCanExit()
    {
        if (!doorIsOpen) return;
        lvlObjective.PlayerExit(type);
        _player.ExitDoor();
        state = false;
        DoorAction = delegate { };
    }

    private IEnumerator WaitToClose()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        anim.Play("Exit_Close_Door");
    }
    public void ShowInteract(bool showInteractState)
    {
        
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.OnOpenDoors, OnOpenDoors);
    }
    private void OnDestroy()
    {
        EventManager.Instance.Unsubscribe(EventType.OnOpenDoors, OnOpenDoors);
    }
}
