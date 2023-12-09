using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class CreateJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInputs, joinInputs;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInputs.text);
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInputs.text);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Testing Level");
    }
}
