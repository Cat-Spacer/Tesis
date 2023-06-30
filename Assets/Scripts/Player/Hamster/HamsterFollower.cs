using UnityEngine;

public class HamsterFollower : MonoBehaviour
{
    [SerializeField] private Hamster _hamster;
    [SerializeField] private Vector3 _offset;

    void Start()
    {
        if (!_hamster)
            _hamster = FindObjectOfType<Hamster>();
        else
            Debug.LogWarning($"There is no player on scene for {name} to follow");
    }

    void Update()
    {
        transform.position = _hamster.transform.position + _offset;
    }
}