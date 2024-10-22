using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class CameraFollow : NetworkBehaviour
{
    private Action _FollowAction = delegate {  };
    private Transform _target;
    private Vector3 _velocity;
    public float smoothTime;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    private void Start()
    {
        EventManager.Instance.Subscribe("OnDisconnectedPlayer", DisconnectPlayer);
        _target = default;
    }

    private void Update()
    {
        _FollowAction();
    }

    void FollowPlayer()
    {
        if (_target == null) return;
        var position = transform.position;
        position = Vector3.SmoothDamp(position, _target.position, ref _velocity, smoothTime);
        position = new Vector3(position.x, position.y, -5);
        transform.position = position;
    }
    [Rpc(SendTo.Everyone)]
    public void StartFollowRpc()
    {
        //_FollowAction += FollowPlayer;
    }
    public void SetCatTransform()
    {
        var cat = FindObjectOfType<CatCharMultiplayer>();
        _target = cat.transform;
        _virtualCamera.Follow = _target;
    }
    [Rpc(SendTo.NotMe)]
    public void SetCatTransformRpc()
    {
        var cat = FindObjectOfType<CatCharMultiplayer>();
        _target = cat.transform;
        _virtualCamera.Follow = _target;
    }
    public void SetHamsterTransform()
    {
        var hamster = FindObjectOfType<HamsterCharMultiplayer>();
        _target = hamster.transform;
        _virtualCamera.Follow = _target;
    }
    [Rpc(SendTo.NotMe)]
    public void SetHamsterTransformRpc()
    {
        var hamster = FindObjectOfType<HamsterCharMultiplayer>();
        _target = hamster.transform;
        _virtualCamera.Follow = _target;
    }
    void DisconnectPlayer(object[] parms)
    {
        _FollowAction = delegate {  };
    }
}
