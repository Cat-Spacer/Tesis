using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    [SerializeField] private PlayerDatas _playerDatas;
    [SerializeField] private bool _active = true;

    private void Awake()
    {
        if (_playerDatas == null)
            _playerDatas = FindObjectOfType<PlayerDatas>();
        else
            Debug.LogWarning($"There's no Player on this level");
    }

    public void SetActive(bool active) { _active = active; }
}
