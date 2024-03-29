using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    #region OLD
    // [Serializable]
    // public struct AreaPoint
    // {
    //     public Transform respawnPoint;
    //     public GameObject[] resetObjects;
    // }
    //
    // public static GameManager Instance;
    // [SerializeField] private Vector3 _respawnPoint;
    // [SerializeField] private AreaPoint[] _respawnArea;
    //
    // private CustomMovement _player;
    //
    // private Action _CounterAction;
    // [Header("DeathScreen")]
    // [SerializeField] private Animator _deathScreen;
    // [SerializeField] private float _resetPlayerTime;
    // [SerializeField] private float _deathScreenTime;
    // int _respawnIndex = 0;
    // float saveDistance;
    // [Header("PickUps")]
    // [SerializeField] private Text _pointsText;
    // [SerializeField] private int _points = 0;
    // [SerializeField] private int _pointsPerLevel = 0;
    // [SerializeField] private List<PickUp> _objectivesInLvl;
    // [SerializeField] private GameObject _winScreen;
    // [SerializeField] private MiniMenu _minuMenu;
    //
    // [Header("CurrentLevel")]
    // [SerializeField] private int _currentLevel = 0;
    // [SerializeField] private MiniMap _miniMap;
    //
    // public bool celestialDiamond = false;
    //
    // private Config _config = null;
    //
    // private void Awake()
    // {
    //     Instance = this;
    //     SoundManager.instance.PauseAll();
    // }
    //
    // private void Start()
    // {
    //     _pointsText.text = _points.ToString() + "/" + _pointsPerLevel.ToString();
    //     _player = FindObjectOfType<CustomMovement>();
    //     _config = GetComponent<Config>();
    //     GetCurrentLevel(0);
    // }
    //
    // private void Update()
    // {
    //     EventSystem.current.SetSelectedGameObject(null);
    // }
    //
    // public GameObject GetPlayer()
    // {
    //     return _player.gameObject;
    // }
    // public void GetCurrentLevel(int lvl)
    // {
    //     _currentLevel = lvl;
    // }
    //
    // public void SaveDistance(float distance)
    // {
    //     saveDistance = distance;
    // }
    //
    // public float ReturnBaseDistanceClimb()
    // {
    //     return saveDistance;
    // }    
    //
    // public void ButtonDie()
    // {
    //     _player.GetDamage();
    // }
    //
    // public void SetNewCheckPoint(Transform newChekPoint)
    // {
    //     _respawnPoint = newChekPoint.position;
    // }
    //
    // public void PlayerDeath ()
    // {
    //     EventManager.Instance.Trigger("PlayerDeath");
    //     _deathScreen.gameObject.SetActive(true);
    //     SoundManager.instance.Play(SoundManager.Types.PlayerDeath);
    //     StartCoroutine(DeathScreenCounter());
    //     StartCoroutine(PlayerResetPositionCounter(_respawnIndex));
    // }
    //
    //
    // public void GetAllObjectivesInLevel(PickUp objective)
    // {
    //     _objectivesInLvl.Add(objective);
    //     if (_objectivesInLvl.Count == _pointsPerLevel)
    //     {
    //         //Debug.Log("Cree");
    //     }
    // }
    //
    // public int ObjectivesToCollect()
    // {
    //     return _pointsPerLevel;
    // }
    //
    // public void SetRespawnPoint(int index_arg)
    // {
    //     _respawnIndex = index_arg;
    // }
    //
    // public void GetItem()
    // {
    //     _points++;
    //     if (_points != _pointsPerLevel)
    //     {
    //         _minuMenu.OpenCall();
    //         _pointsText.text = _points.ToString() + " / " + _pointsPerLevel.ToString();
    //         _miniMap.GotItem();
    //     }
    //     else //Ganaste
    //     {
    //         _winScreen.SetActive(true);
    //         if (FindObjectOfType<FadeInOut>() && FindObjectOfType<ZoomEffect>())
    //         {
    //             var camera = FindObjectOfType<ZoomEffect>().gameObject;
    //             var bg = FindObjectOfType<FadeInOut>().gameObject;
    //             camera.SetActive(false);
    //             bg.SetActive(false);
    //         }
    //     }
    // }
    //
    // public void EndGame()
    // {
    //     _winScreen.SetActive(true);
    // }
    // public void WaitForEndClimb(float waitTime)
    // {
    //     StartCoroutine(CoroutineWaitForEndClimb(waitTime));
    // }
    //
    // public Config GetConfig { get { return _config; } }
    //
    // IEnumerator PlayerResetPositionCounter(int index_arg)
    // {
    //     yield return new WaitForSeconds(_resetPlayerTime);
    //     _player.transform.position = new Vector3(_respawnPoint.x, _respawnPoint.y, 0);
    //     _player.ResetPlayer();
    // }
    //
    // IEnumerator DeathScreenCounter()
    // {
    //     yield return new WaitForSeconds(_deathScreenTime);
    //     _deathScreen.gameObject.SetActive(false);
    //     _player.ConstrainsReset();
    // }
    //
    // public IEnumerator CoroutineWaitForEndClimb(float waitTime)
    // {
    //     yield return new WaitForSeconds(waitTime);
    //     //Climb.isClimbing = false;        
    // }
    #endregion
    public static GameManager Instance;
    [SerializeField] private CatChar catChar;
    [SerializeField] private HamsterChar hamsterChar;
    [SerializeField] private DoubleLinkedList<PlayerCharacter> _players = default;

    public DoubleLinkedList<PlayerCharacter> GetPlayers { get => _players; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void SetChar(List<PlayerFA> playerFa)
    {
        foreach (var player in playerFa)
        {
            if (player.GetComponent<CatChar>()) catChar = player.GetComponent<CatChar>();
            else hamsterChar = player.GetComponent<HamsterChar>();
        }
    }
    public void SetCat(CatChar cat)
    {
        catChar = cat;
    }
    public void SetHamster(HamsterChar hamster)
    {
        hamsterChar = hamster;
    }

    public void SetPlayer(PlayerCharacter player) 
    {
        if (!player || _players.Count < 2 || _players.Contains(player)) return;
        _players.Add(player);
    }

    public CatChar GetCatChar()
    {
        return catChar;
    }
    public HamsterChar GetHamsterChar()
    {
        return hamsterChar;
    }

    
}
