using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{
    public GameObject mainScreen, connectedScreen;

    private void Start()
    {
        BTN_Connect();
    }

    public void BTN_Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("CreateJoinLobby");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Connection failed: {cause.ToString()}");
    }
}
