using Photon.Realtime;
using System.Linq;
using UnityEngine;

public class CameraReadjust : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private DoubleLinkedList<PlayerCharacter> _players;
    [SerializeField] private float _minZoom = 12.5f, _maxZoom = 31.45f;

    private void Awake()
    {
        if (!_camera) _camera = GetComponent<Camera>();
    }

    void Start()
    {
        if (_players.Count < 2) _players = GameManager.Instance.GetPlayers;
    }

    void Update()
    {
        if (_players.Count < 2) return;
        float playersDis = 0.0f, actualZoom = 0.0f;
        playersDis = Vector3.Distance(_players[0].transform.position, _players[1].transform.position);
        actualZoom = Mathf.Lerp(_minZoom,_maxZoom,playersDis);

        if (_camera.orthographic) _camera.fieldOfView = actualZoom;
    }
}