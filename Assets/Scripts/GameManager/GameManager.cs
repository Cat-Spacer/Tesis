using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField] private float mouseIdleTimer;
    [SerializeField] private float checkInterval;
    private Vector3 lastMousePosition;
    private bool isMouseVisible = true;
    private bool onStopCursor;
    private void Awake()
    {
        Time.timeScale = 0;
        if (Instance == null) Instance = this;
        _respawnManager = GetComponentInChildren<Respawn>();
        if (catPlayer == null) catPlayer = FindObjectOfType<CatCharacter>();
        if (hamsterPlayer == null) hamsterPlayer = FindObjectOfType<HamsterChar>();
        lastMousePosition = Input.mousePosition;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Start()
    {
        GetLevels();
        StartCoroutine(CheckMouseMovement());
        EventManager.Instance.Subscribe(EventType.OnLoseGame, OnLoseGame);
        EventManager.Instance.Subscribe(EventType.OnFinishGame, OnFinishGame);
        EventManager.Instance.Subscribe(EventType.OnResumeGame, OnResumeGame);
        EventManager.Instance.Subscribe(EventType.OnPauseGame, OnPauseGame);
    }
    private void OnPauseGame(object[] obj)
    {
        Cursor.visible = true;
        onStopCursor = true;
    }
    private void OnResumeGame(object[] obj)
    {
        onStopCursor = false;
        StartCoroutine(CheckMouseMovement());
    }
    private void OnFinishGame(object[] obj)
    {
        Cursor.visible = true;
        onStopCursor = true;
    }
    private void OnLoseGame(object[] obj)
    {
        Cursor.visible = true;
        onStopCursor = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) if(EventManager.Instance != null) EventManager.Instance.Trigger(EventType.ViewPlayerIndicator, true);
        if (Input.GetKeyUp(KeyCode.Tab))  if(EventManager.Instance != null) EventManager.Instance.Trigger(EventType.ViewPlayerIndicator, false);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.R)) EventManager.Instance.Trigger(EventType.OnLoseGame);
            if (Input.GetKeyDown(KeyCode.C)) KillPlayer(CharacterType.Cat);
            if (Input.GetKeyDown(KeyCode.H)) KillPlayer(CharacterType.Hamster);
        }
    }
    IEnumerator CheckMouseMovement()
    {
        float idleTimer = 0f;
        while (!onStopCursor)
        {
            if (Input.mousePosition != lastMousePosition)
            {
                idleTimer = 0f;

                if (!isMouseVisible)
                {
                    Cursor.visible = true;
                    isMouseVisible = true;
                }
            }
            else
            {
                idleTimer += checkInterval;
                if (idleTimer >= mouseIdleTimer && isMouseVisible)
                {
                    Cursor.visible = false;
                    isMouseVisible = false;
                }
            }

            // Actualiza la última posición del mouse.
            lastMousePosition = Input.mousePosition;

            // Espera el intervalo antes del siguiente chequeo.
            yield return new WaitForSeconds(checkInterval);
        }
    }
    void KillPlayer(CharacterType type)
    {
        if(type == CharacterType.Cat) catPlayer.GetDamage();
        else hamsterPlayer.GetDamage();
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
    public void WinLevel()
    {
        Time.timeScale = 0f;
        EventManager.Instance.Trigger(EventType.OnFinishGame);
        Cursor.visible = true;
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
        EventManager.Instance.Trigger(EventType.OnStartGame, true);
        Time.timeScale = 1f;
        SoundManager.instance.Play(SoundsTypes.Music, gameObject,true);
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
    public Transform GetCat()
    {
        return catPlayer.transform;
    }

    public Transform GetHamster()
    {
        return hamsterPlayer.transform;
    }
}