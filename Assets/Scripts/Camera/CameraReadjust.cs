using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraReadjust : NetworkBehaviour
{
    private Action _movementAction;
    [SerializeField] private Camera _camera;
    [SerializeField] private List<PlayerCharacterMultiplayer> characters = new List<PlayerCharacterMultiplayer>();
    [SerializeField] private float _minZoom = 5, _maxZoom = 10f;

    public Vector3 offset;
    private Vector3 _velocity;
    public float smoothTime = .5f;
    public float zoomLimiter;
    private void Awake()
    {
        if (!_camera) _camera = GetComponent<Camera>();
        _movementAction = CheckPlayers;
    }

    void Start()
    {
        characters.Clear();
    }
    void LateUpdate()
    {
        _movementAction();
    }

    void Move()
    {
        if (characters.Count == 0) return;
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPos = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref _velocity, smoothTime);
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(_minZoom, _maxZoom, GetGreatesDistance() / zoomLimiter);
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, newZoom, Time.deltaTime);
    }

    float GetGreatesDistance()
    {
        var bounds = new Bounds(characters[0].transform.position, Vector3.zero);
        for (int i = 0; i < characters.Count; i++)
        {
            bounds.Encapsulate(characters[i].transform.position);
        }
        return bounds.size.x;
    }
    Vector3 GetCenterPoint()
    {
        if (characters.Count == 1)
        {
            return characters[0].transform.position;
        }

        var bounds = new Bounds(characters[0].transform.position, Vector3.zero);
        for (int i = 0; i < characters.Count; i++)
        {
            bounds.Encapsulate(characters[i].transform.position);   
        }

        return bounds.center;
    }

    void CheckPlayers()
    {
        if (IsServer)
        {
            if (characters.Count != 2) return;
            StartMovementRpc();
        }
    }
    [Rpc(SendTo.Everyone)]
    void StartMovementRpc()
    {
        _movementAction = Move;
        _movementAction += Zoom;
    }
    [Rpc(SendTo.Everyone)]
    public void SetCatCharRpc()
    {
        var player = FindObjectOfType<CatCharMultiplayer>();
        //if (characters.Contains(player)) return;
        characters.Add(player);
    }
    [Rpc(SendTo.Everyone)]
    public void SetHamsterCharRpc()
    {
        var player = FindObjectOfType<HamsterCharMultiplayer>();
        //if (characters.Contains(player)) return;
        characters.Add(player);
    }
} 