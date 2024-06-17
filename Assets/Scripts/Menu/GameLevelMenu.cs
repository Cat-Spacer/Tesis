using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLevelMenu : NetworkBehaviour
{
    [SerializeField] private Button _resumeBtn;
    [SerializeField] private Button _optionsBtn;
    [SerializeField] private Button _levelMenuBtn;
    [SerializeField] private Button _menuBtn;
    
    [SerializeField] private GameObject _pauseMenu;

    
    private void Awake()
    {
        _resumeBtn.onClick.AddListener(Resume);
        _optionsBtn.onClick.AddListener(Options);
        _levelMenuBtn.onClick.AddListener(LevelMenu);
        _menuBtn.onClick.AddListener(MainMenu);
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallBack;
    }

    private void NetworkManager_OnClientDisconnectCallBack(ulong clientId)
    {
        if (clientId == NetworkManager.ServerClientId)
        {
            NetworkManager.Singleton.DisconnectClient(clientId);
            SceneManager.LoadScene("MenuMultiplayer", LoadSceneMode.Single);
        }
        else
        {
            MainMenuRpc();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pause");
            PauseMenu();
        }
    }

    void Resume()
    {
        _pauseMenu.SetActive(false);
        ResetMenu();
    }
    void Options()
    {
        
    }

    void LevelMenu()
    {
        if (IsServer)
        {
            
        }
    }

    [Rpc(SendTo.Server)]
    void LevelMenuRpc()
    {
        
    }
    void MainMenu()
    {
        MainMenuRpc();
    }
    [Rpc(SendTo.Server)]
    void MainMenuRpc()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("MenuMultiplayer", LoadSceneMode.Single);
    }
    void PauseMenu()
    {
        _pauseMenu.SetActive(true);
    }
    void ResetMenu()
    {
        
    }
}
