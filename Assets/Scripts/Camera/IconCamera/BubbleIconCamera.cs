using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class BubbleIconCamera : MonoBehaviour
{
    [SerializeField] private Vector2 _offset = default;
    [SerializeField] private float _with = 0f, _height = 0f;
    [SerializeField] private Transform _target = default;
    private Vector2 _screenBounds = default;

    private void Awake()
    {

    }

    void Start()
    {
        _screenBounds = new Vector2(Screen.width / 2, Screen.height / 2);
        _height = GetComponent<RectTransform>().rect.height;
        _with = GetComponent<RectTransform>().rect.width;
    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, -_screenBounds.x + _with + _offset.x, _screenBounds.x - (_with + _offset.x));
        viewPos.y = Mathf.Clamp(viewPos.y, -_screenBounds.y + _height + _offset.y, _screenBounds.y - (_height + _offset.y));
        if ((viewPos.x > _screenBounds.x / 2 && viewPos.x < -_screenBounds.x / 2) || (viewPos.y > _screenBounds.y / 2 && viewPos.y < -_screenBounds.y / 2)) return;
        _target.position = Camera.main.WorldToScreenPoint(_target.position);
        transform.position = _target.position;
        transform.position = viewPos;
    }
}