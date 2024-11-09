using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameLevelMenu menu;
    private Respawn _respawnManager;
    [SerializeField] private PlayerCharacter catPlayer, hamsterPlayer;
    [SerializeField] string  _level;
    [SerializeField] string  _nextLevel;
    [SerializeField] private bool testing;
    [SerializeField] private StartDoor[] _startDoors;
    [SerializeField] private List<StaticScreen> _staticScreens = default;
    [SerializeField] private List<PlayerCharacter> _gamePlayers = default;

    public List<PlayerCharacter> AddPlayer { get { return _gamePlayers; } set { _gamePlayers = value; } }
   
    private void Awake()
    {
        if (Instance == null) Instance = this;
        _respawnManager = GetComponentInChildren<Respawn>();
        
    }

    private void Start()
    {
        if (testing)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
        GetLevels();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) if(EventManager.Instance != null) EventManager.Instance.Trigger(EventType.ViewPlayerIndicator, true);
        if(Input.GetKeyUp(KeyCode.Tab))  if(EventManager.Instance != null) EventManager.Instance.Trigger(EventType.ViewPlayerIndicator, false);
    }

    void GetLevels()
    {
        var currentLevel = SceneManager.GetActiveScene();
        _level = currentLevel.name;
        int nextLevelIndex = currentLevel.buildIndex + 1;
        if (nextLevelIndex < SceneManager.sceneCountInBuildSettings)
        {
            _nextLevel = SceneUtility.GetScenePathByBuildIndex(nextLevelIndex);
            _nextLevel = System.IO.Path.GetFileNameWithoutExtension(_nextLevel);
        }
        else _nextLevel = "No hay siguiente nivel"; // O manejar este caso como prefieras
    }

    public string GetCurrentLevelName()
    {
        return _level;
    }
    public string GetNextLevelName()
    {
        return _nextLevel;
    }
    public void WinLevel()
    {
        Time.timeScale = 0f;
        menu.WinMenu();
    }
    public void StartGame(SO_Inputs catInputs, SO_Inputs hamsterInputs)
    {
        catPlayer.gameObject.SetActive(true);
        hamsterPlayer.gameObject.SetActive(true);
        foreach (var door in _startDoors)
        {
            if (door.GetPlayerType() == CharacterType.Cat)
            {
                catPlayer.transform.position = door.GetPlayerPosition().position;
                catPlayer.ReceiveInputs(catInputs);
            }
            else
            {
                hamsterPlayer.transform.position = door.GetPlayerPosition().position;
                hamsterPlayer.ReceiveInputs(hamsterInputs);
            }
            door.Open();
        }
        if(LiveCamera.instance != null) LiveCamera.instance.StartLiveCamera(true);
        Time.timeScale = 1f;
    }
    public void SetCatRespawnPoint(Vector3 pos)
    {
        _respawnManager.SetCatRespawnPoint(pos);
    }
    public void SetHamsterRespawnPoint(Vector3 pos)
    {
        _respawnManager.SetHamsterRespawnPoint(pos);
    }
    public Vector3 GetCatRespawnPoint()
    {
        return _respawnManager.GetCatRespawnPoint();
    }
    public Vector3 GetHamsterRespawnPoint()
    {
        return _respawnManager.GetHamsterRespawnPoint();
    }
    public List<StaticScreen> StaticScreens(StaticScreen value = default)
    {
        if (value) _staticScreens.Add(value);
        return _staticScreens;
    }

    public Transform GetCat()
    {
        return catPlayer.transform;
    }

    public Transform GetHamster()
    {
        return hamsterPlayer.transform;
    }
}