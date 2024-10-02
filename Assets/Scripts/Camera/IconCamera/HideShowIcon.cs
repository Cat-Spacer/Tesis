using UnityEngine;

public class HideShowIcon : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _playerToFollow = default, _myPlayer = default;
    [SerializeField] private float _margin = 1f;
    private Vector2 _screenBounds = default;

    public PlayerCharacter MyPlayer { get { return _myPlayer; } }

    void Start()
    {
        _myPlayer = GetComponentInParent<PlayerCharacter>();
        if (!_myPlayer) Destroy(this);
        CheckForPlayer();
    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, _screenBounds.x + _margin, -(_screenBounds.x + _margin));
        viewPos.y = Mathf.Clamp(viewPos.y, _screenBounds.y + _margin, -(_screenBounds.y + _margin));

        if ((viewPos.x > _screenBounds.x && viewPos.x < 0) && (viewPos.x > _screenBounds.x && viewPos.x < 0))
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private void CheckForPlayer()
    {
        if ((!_playerToFollow || _playerToFollow == this) && GameManager.instance)
            foreach (var player in GameManager.instance.AddPlayer)
                if (player != _myPlayer) _playerToFollow = player;
    }
}