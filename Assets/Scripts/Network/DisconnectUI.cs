using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DisconnectUI : NetworkBehaviour
{
    [SerializeField] private LevelMenu _levelMenu;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject hostText, clientText, disconnected;
    void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallBack;
        //NetworkManager.Singleton.OnClientStopped += OnServerStopped;
        //NetworkManager.Singleton.OnServerStopped 
        Hide();
        hostText.SetActive(false);
        clientText.SetActive(false);
    }
    private void NetworkManager_OnClientDisconnectCallBack(ulong clientId)
    {
        Show("host");
    }
    void OnServerStopped(bool stopped)
    {
        Debug.Log("Stopped");
        if (stopped)
        {
            OnHostDisconnectRpc();
        }
        
    }

    [Rpc(SendTo.Everyone)]
    void OnHostDisconnectRpc()
    {
        Show("host");
    }
    public void Show(string key)
    {
        menu.SetActive(true);
        if(key == "host") disconnected.SetActive(true);
        else disconnected.SetActive(true);
    }

    public void Hide()
    {
        menu.SetActive(false);
        hostText.SetActive(false);
        clientText.SetActive(false);
    }
}
