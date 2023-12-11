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
    private bool playerHasSelected = false;

    public List<PlayerFA> players = new List<PlayerFA>();

    [SerializeField] private PlayerFA catCharacter;
    [SerializeField] private PlayerFA hamsterCharacter;

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
        var player = PhotonNetwork.LocalPlayer;
        if (catSelected || playerHasSelected) return;
        playerHasSelected = true;
        catCharacter = PhotonNetwork.Instantiate(catPrefab.name, catSpawnPoint.position, Quaternion.identity)
            .GetComponent<PlayerFA>()
            .SetInitialParameters(player, player.ActorNumber);
        photonView.RPC("SelectCat", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }
    public void SelectHamsterButton()
    {
        if (hamsterSelected || playerHasSelected) return;
        playerHasSelected = true;
        var player = PhotonNetwork.LocalPlayer;
        hamsterCharacter = PhotonNetwork.Instantiate(hamsterPrefab.name, hamsterSpawnPoint.position, Quaternion.identity)
            .GetComponent<PlayerFA>()
            .SetInitialParameters(player, player.ActorNumber);
        photonView.RPC("SelectHamster", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }
    [PunRPC]
    public void SelectCat(Player player)
    {
        //players.Add(catCharacter);
        var cat = FindObjectOfType<CatChar>();
        //var cat = catCharacter.gameObject.GetComponent<CatChar>();
        if(cat != null) GameManager.Instance.SetCat(cat);
        PowerUpManager.instance.cat = cat.GetComponent<CatSpecial>();
        catText.text = "Player " + player.ActorNumber;
        catSelected = true;
    }
    [PunRPC]
    public void SelectHamster(Player player)
    {
        //players.Add(hamsterCharacter);
        var hamster = FindObjectOfType<HamsterChar>();
        //var hamster = hamsterCharacter.gameObject.GetComponent<HamsterChar>();
        if(hamster != null) GameManager.Instance.SetHamster(hamster);
        hamsterText.text = "Player " + player.ActorNumber;
        hamsterSelected = true;
    }
}
