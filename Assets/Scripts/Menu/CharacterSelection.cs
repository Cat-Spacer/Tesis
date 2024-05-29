using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private Button _catBtn, _hamsterBtn;
    [SerializeField] private SpriteRenderer _player1, _player2;
    private bool _catSelected, _hamsterSelected;
    
    //
    // public void SelectCat()
    // {
    //     SelectCatRpc();
    // }
    // [Rpc(SendTo.Everyone)]
    // void SelectCatRpc(RpcParams rpcParams = default)
    // {
    //     var clientId = rpcParams.Receive.SenderClientId;
    //     var player = Instantiate(_catPrefab);
    //     _catChar = player.gameObject.GetComponent<PlayerCharacterMultiplayer>();
    //     _catChar.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    //     _catChar.transform.position = _catStartingPos.position;
    //     _camera.SetCatCharRpc(_catChar);
    // }
    // public void SelectHamster()
    // {
    //     SelectHamsterRpc();
    // }
    // [Rpc(SendTo.Everyone)]
    // void SelectHamsterRpc(RpcParams rpcParams = default)
    // {
    //     var clientId = rpcParams.Receive.SenderClientId;
    //     var player = Instantiate(_hamsterPrefab);
    //     _hamsterChar = player.gameObject.GetComponent<PlayerCharacterMultiplayer>();
    //     _hamsterChar.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    //     _hamsterChar.transform.position = _hamsterStartingPos.position;
    //     _camera.SetHamsterCharRpc(_hamsterChar);
    // }
    //
    // public void StartGame()
    // {
    //     StartGameRpc();
    // }
    // [Rpc(SendTo.Everyone)]
    // void StartGameRpc()
    // {
    //     
    // }
}
