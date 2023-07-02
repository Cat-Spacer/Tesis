using UnityEngine;

public class HamsterFollower : MonoBehaviour
{
    [SerializeField] private Hamster _hamster;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private bool _canvas = false;
    private RectTransform _rt;

    void Start()
    {
        if (!_hamster)
            _hamster = FindObjectOfType<Hamster>();
        else
            Debug.LogWarning($"There is no player on scene for {name} to follow");
        if (_canvas) _rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        if(!_hamster) return;
        if (_rt)
        {
            Vector3 targPos = _hamster.transform.position;
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camPos = Camera.main.transform.position + camForward;
            float distInFrontOfCamera = Vector3.Dot(targPos - camPos, camForward);
            if (distInFrontOfCamera < 0f)
            {
                targPos -= camForward * distInFrontOfCamera;
            }
            transform.position = (Vector3)RectTransformUtility.WorldToScreenPoint(Camera.main, targPos) + _offset;
        }
        else
            transform.position = _hamster.transform.position + _offset;
    }
}