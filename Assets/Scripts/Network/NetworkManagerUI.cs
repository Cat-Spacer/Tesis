using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    private void Awake()
    {
        serverButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }

    private void Update()
    {
        if (!IsHost) return;
        if (NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            OpenLevelClientRpc();
        }
    }

    [ClientRpc]
    void OpenLevelClientRpc()
    {
        NetworkManager.SceneManager.LoadScene("MultiplayerTesting", LoadSceneMode.Single);
    }
}
