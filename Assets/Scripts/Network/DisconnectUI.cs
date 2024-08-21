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
        Hide();
        hostText.SetActive(false);
        clientText.SetActive(false);
    }
    private void NetworkManager_OnClientDisconnectCallBack(ulong clientId)
    {
        Show("host");
        if(clientId == NetworkObject.OwnerClientId) NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallBack;
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

    public void Unsubscribe()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallBack;
    }
}
