using UnityEngine;

public class TargetPointer : MonoBehaviour
{
    [SerializeField] private Transform _target = default;
    [SerializeField] private RectTransform _pointerRect = default;

    private void Start()
    {
        if (!_target) _target = GetComponentInParent<HideShowIcon>().MyPlayer.transform;
    }

    void Update()
    {
        Vector2 target = _target.position;

        Vector2 cameraPos = Camera.main.transform.position;

        Vector2 dir = (target - cameraPos).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _pointerRect.localEulerAngles = new Vector3(0, 0, angle - 90);
    }
}