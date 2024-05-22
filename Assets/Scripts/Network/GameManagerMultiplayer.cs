using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public class GameManagerMultiplayer : NetworkBehaviour
{
    [SerializeField] private Transform _catPrefab, _hamsterPrefab;
    [SerializeField] private Transform _catStartingPos, _hamsterStartingPos;

    private void Update()
    {

    }

    public void SpawnCat()
    {
        if (IsServer)
        {
            var player = Instantiate(_catPrefab);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId);
            player.position = _catStartingPos.position;
        }
        else
        {
            SpawnCatServerRpc();
        }
    }

    public void SpawnHamster()
    {
        if (IsServer)
        {
            var player = Instantiate(_hamsterPrefab);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId);
            player.position = _hamsterStartingPos.position;
        }
        else
        {
            SpawnHamsterServerRpc();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void SpawnCatServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        var player = Instantiate(_catPrefab);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        player.position = _catStartingPos.position;
    } 
    [ServerRpc(RequireOwnership = false)]
    private void SpawnHamsterServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        var player = Instantiate(_hamsterPrefab);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        player.position = _hamsterStartingPos.position;
    } 
}
