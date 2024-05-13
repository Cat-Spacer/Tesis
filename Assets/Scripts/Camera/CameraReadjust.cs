using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraReadjust : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private DoubleLinkedList<PlayerCharacter> _players;
    [SerializeField] private List<PlayerCharacter> characters;
    [SerializeField] private float _minZoom = 5, _maxZoom = 10f;

    public Vector3 offset;
    private Vector3 _velocity;
    public float smoothTime = .5f;
    public float zoomLimiter;
    private void Awake()
    {
        if (!_camera) _camera = GetComponent<Camera>();
    }

    void Start()
    {
        //if (_players.Count < 2) _players = GameManager.Instance.GetPlayers;
        
        characters = GameManager.Instance.GetCharacters();
    }
    void LateUpdate()
    {
        Move();
        Zoom();
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
} 