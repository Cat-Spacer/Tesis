using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerFA : MonoBehaviourPun
{
    Player _owner;
    [SerializeField] int playerId;
    private PlayerData _data;
    
    public PlayerFA SetInitialParameters(Player player, int id)
    {
        _owner = player;
        
        //photonView.RPC("SetLocalParms", _owner, _currentLife);
        
        photonView.RPC("RPCSetId", RpcTarget.AllBuffered, id);

        return this;
    }
    [PunRPC]
    void RPCSetId(int id)
    {
        playerId = id;
    }
}
