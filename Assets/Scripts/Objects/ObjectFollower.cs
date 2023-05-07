using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    [SerializeField] private GameObject _object;
    [SerializeField] private Vector3 _offset;

    void Update()
    {
        if (_object)
            transform.position = _object.transform.position + _offset;
    }
}
