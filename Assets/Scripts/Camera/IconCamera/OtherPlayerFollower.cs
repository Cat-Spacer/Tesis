using UnityEngine;

public class OtherPlayerFollower : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _playerToFollow = default, _myPlayer = default;
    [SerializeField] private Vector3 _offset = default;

    void Start()
    {
        _myPlayer = GetComponentInParent<PlayerCharacter>();
        if (!_myPlayer) Destroy(this);
        CheckForPlayer();
    }

    void Update()
    {
        if(!_playerToFollow) CheckForPlayer();

        transform.position = _offset + _playerToFollow.transform.position;
    }

    private void CheckForPlayer()
    {
        if ((!_playerToFollow || _playerToFollow == this) && GameManager.Instance)
            foreach (var player in GameManager.Instance.AddPlayer)
                if (player != _myPlayer) _playerToFollow = player;
    }
}