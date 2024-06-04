using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Netcode;
using UnityEngine;

public class FlagChekpointNetwork : MonoBehaviour
{
    [SerializeField] private GameObject _onFlag, _offFlag;
    [SerializeField] private Vector2 _boxArea;    
    [SerializeField] private LayerMask _players;
    private Animator _anim;
    private bool _isOn = false;
    void Start()
    {
        _anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (_isOn) return;
        var coll = Physics2D.OverlapBox(transform.position, _boxArea, 0, _players);
        if (coll == null) return;
        var networkObj = coll.GetComponent<NetworkObject>();
        if (networkObj == null) return;
        RespawnPointRpc(networkObj);
    }

    [Rpc(SendTo.Everyone)]
    void RespawnPointRpc(NetworkObjectReference flag)
    {
        flag.TryGet(out NetworkObject flagNetworkObject);
        Debug.Log("FlagOn");
        _isOn = true;
        _anim.SetTrigger("ON");
        GameManagerNetwork.Instance.SetRespawnPoint(flagNetworkObject.transform.position);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, _boxArea);
    }

}
