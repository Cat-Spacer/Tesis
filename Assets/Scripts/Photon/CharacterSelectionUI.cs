using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class CharacterSelectionUI : MonoBehaviourPunCallbacks
{
    public GameObject catPrefab, hamsterPrefab;
    public GameObject characterSelection;
    public Transform catSpawnPoint, hamsterSpawnPoint;
    public GameObject waitingForPlayer;
    public TextMeshProUGUI catText, hamsterText;
    bool playersReady = false;
    bool catSelected;
    bool hamsterSelected;

    List<PlayerFA> players = new List<PlayerFA>();

    private void Update()
    {
        if(!photonView.IsMine) return;
        if (PhotonNetwork.PlayerList.Length == 2 && !playersReady)
        {
            photonView.RPC("OnCharacterSelection", RpcTarget.All);
        }
        if(catSelected && hamsterSelected)
        {
            photonView.RPC("OnCharacterSelected", RpcTarget.All);
        }
    }
    [PunRPC]
    void OnCharacterSelection()
    {
        playersReady = true;
        waitingForPlayer.SetActive(false);
        characterSelection.SetActive(true);
    }

    [PunRPC]
    void OnCharacterSelected()
    {
        gameObject.SetActive(false);
    }
    public void SelectCatButton()
    { 
        if(!catSelected) photonView.RPC("SelectCat", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }
    public void SelectHamsterButton()
    {
        if(!hamsterSelected) photonView.RPC("SelectHamster", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }
    [PunRPC]
    public void SelectCat(Player player)
    {
        catText.text = "Player " + player.ActorNumber;
        catSelected = true;

        PlayerFA newCharacter = PhotonNetwork.Instantiate(catPrefab.name, catSpawnPoint.position, Quaternion.identity)
                                .GetComponentInChildren<PlayerFA>()
                                .SetInitialParameters(player, player.ActorNumber);
                                players.Add(newCharacter);
    }
    [PunRPC]
    public void SelectHamster(Player player)
    {
        hamsterText.text = "Player " + player.ActorNumber;
        hamsterSelected = true;

        PlayerFA newCharacter = PhotonNetwork.Instantiate(hamsterPrefab.name, hamsterSpawnPoint.position, Quaternion.identity)
                        .GetComponentInChildren<PlayerFA>()
                        .SetInitialParameters(player, player.ActorNumber);
        players.Add(newCharacter);
    }
}
