using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class CharacterSelectionMenu : NetworkBehaviour
{
    [SerializeField] private Transform _catPrefab, _hamsterPrefab;
    [SerializeField] private PlayerCharacterMultiplayer _catChar, _hamsterChar;
    //[SerializeField] private NetworkVariable<PlayerCharacterMultiplayer> _catCharNetwork = new NetworkVariable<PlayerCharacterMultiplayer>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //[SerializeField] private NetworkVariable<PlayerCharacterMultiplayer> _hamsterCharNetwork = new NetworkVariable<PlayerCharacterMultiplayer>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] private Transform _catStartingPos, _hamsterStartingPos;
    public static CharacterSelectionMenu Instance;
    public CameraReadjust _camera;

    [SerializeField] private GameObject _charaSelection;
    [SerializeField] private TextMeshProUGUI _player1SelectedText, _player2SelectedText;
    private bool _player1CatSelected, _player1HamsterSelected;
    private bool _player2CatSelected, _player2HamsterSelected;
    [SerializeField] private Button _confirmBtn;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        _camera = Camera.main.GetComponent<CameraReadjust>();
    }


    // public PlayerCharacterMultiplayer GetCatChar()
    // {
    //     if (_catCharNetwork.Value != null) return _catCharNetwork.Value;
    //     else return default;
    // }
    // public PlayerCharacterMultiplayer GetHamsterChar()
    // {
    //     if (_hamsterCharNetwork.Value != null) return _hamsterCharNetwork.Value;
    //     else return default;
    // }
    #region Network

    public void ConfirmCharacters()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            //ConfirmCharactersRpc();
            SpawnCat();
            SpawnHamster();
            ConfirmCharactersRpc();
            _camera.SetCatCharRpc();
            _camera.SetHamsterCharRpc();
            GameManagerNetwork.Instance.StartGame();
        }
    }
    
    #region Selection
    [Rpc(SendTo.Everyone)]
    void ConfirmCharactersRpc()
    {
        _charaSelection.SetActive(false);
    }
    public void SelectCat()
    {
        if (NetworkManager.Singleton.IsServer) P1SelectCatRpc();
        else P2SelectCatRpc();
    }
    public void SelectHamster()
    {
        if (NetworkManager.Singleton.IsServer) P1SelectHamsterRpc();
        else P2SelectHamsterRpc();
    }
    [Rpc(SendTo.Everyone)]
    void P1SelectCatRpc()
    {
        _player1CatSelected = true;
        _player1HamsterSelected = false;

        if (!_player2CatSelected) _player1SelectedText.rectTransform.localPosition = new Vector3(-300, -215);
        else _player1SelectedText.rectTransform.localPosition = new Vector3(-300, -280);
    }   
    [Rpc(SendTo.Everyone)]
    void P2SelectCatRpc()
    {
        _player2CatSelected = true;
        _player2HamsterSelected = false;
            
        if (!_player1CatSelected) _player2SelectedText.rectTransform.localPosition = new Vector3(-300, -215);
        else _player2SelectedText.rectTransform.localPosition = new Vector3(-300, -280);
    }
    [Rpc(SendTo.Everyone)]
    void P1SelectHamsterRpc()
    {
        _player1CatSelected = false;
        _player1HamsterSelected = true;
            
        if (!_player2HamsterSelected) _player1SelectedText.rectTransform.localPosition = new Vector3(300, -215);
        else _player1SelectedText.rectTransform.localPosition = new Vector3(300, -280);
    }    
    [Rpc(SendTo.Everyone)]
    void P2SelectHamsterRpc()
    {
        _player2CatSelected = false;
        _player2HamsterSelected = true;
            
        if (!_player1HamsterSelected) _player2SelectedText.rectTransform.localPosition = new Vector3(300, -215);
        else _player2SelectedText.rectTransform.localPosition = new Vector3(300, -280);
    }
    #endregion Selection
    
    #region Spawn
    void SpawnCat()
    {
        if (_player1CatSelected)
        {
            _catChar = Instantiate(_catPrefab).gameObject.GetComponent<PlayerCharacterMultiplayer>();
            _catChar.GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId);
            _catChar.transform.position = _catStartingPos.position;
            //_camera.SetCatCharRpc(_catChar);
        }
        else
        {
            _catChar = Instantiate(_catPrefab).gameObject.GetComponent<PlayerCharacterMultiplayer>();
            _catChar.GetComponent<NetworkObject>().SpawnAsPlayerObject(1);
            _catChar.transform.position = _catStartingPos.position;
        }
        //_camera.SetCatCharRpc();
        //GameManagerNetwork.Instance.SetCatChar(_catChar);
    }

    void SpawnHamster()
    {
        if (_player1HamsterSelected)
        {
            _hamsterChar = Instantiate(_hamsterPrefab).gameObject.GetComponent<PlayerCharacterMultiplayer>();
            _hamsterChar.GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId);
            _hamsterChar.transform.position = _hamsterStartingPos.position;
            //_camera.SetHamsterCharRpc(_hamsterChar);
        }
        else
        {
            _hamsterChar = Instantiate(_hamsterPrefab).gameObject.GetComponent<PlayerCharacterMultiplayer>();
            _hamsterChar.GetComponent<NetworkObject>().SpawnAsPlayerObject(1);
            _hamsterChar.transform.position = _hamsterStartingPos.position;
            //_camera.SetHamsterCharRpc(_hamsterChar);
        }

        //GameManagerNetwork.Instance.SetHamsterChar(_hamsterChar);
    }
    #endregion Spawn
    
    #endregion Network
}
