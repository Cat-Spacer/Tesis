using UnityEngine;

public class ZoomEffect : MonoBehaviour
{
    [SerializeField] private float _zoom = 2f;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector2 _size = new Vector2();
    [SerializeField] private Rect _wantedRect = new Rect();

    private BoxCollider2D _boxCollider2D;
    private CircleCollider2D _circleCollider2D;

    [HideInInspector] public Rect orgRect = new Rect();
    private Vector3 _orgScale = Vector3.one;
    private Plane _plane = new Plane(Vector3.back, 0);

    private void Start()
    {
        if (!_rectTransform) _rectTransform = GetComponent<RectTransform>();

        if (!_camera) _camera = GetComponent<Camera>();

        if (_camera) orgRect = _camera.rect;

        if (!_circleCollider2D && !_camera)
        {
            _circleCollider2D = GetComponent<CircleCollider2D>();
            if (_rectTransform && _circleCollider2D)
            {
                _orgScale = _rectTransform.localScale;
                _circleCollider2D.radius = _rectTransform.rect.width / 2.0f;
            }
        }

        if (!_boxCollider2D && !_camera)
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
            if (_rectTransform && _boxCollider2D)
            {
                _orgScale = _rectTransform.localScale;
                _boxCollider2D.size = new Vector2(_rectTransform.rect.width, _rectTransform.rect.height);
            }
        }
    }

    private void Update() { CantUseMouseOver(); }

    private void CantUseMouseOver()
    {
        if (!_camera || _camera.rect.x == 0 || _camera.rect.y == 0) return;

        var mPos = Input.mousePosition;
        var ray = _camera.ScreenPointToRay(mPos);
        var worldPos = new Vector3();

        if (_plane.Raycast(ray, out float distance)) worldPos = ray.GetPoint(distance);

        var dist = Vector2.Distance(worldPos, _camera.transform.position);

        if (dist < _size.x && dist < _size.y) _camera.rect = _wantedRect;
        else _camera.rect = orgRect;
    }

    private void OnMouseOver()
    {
        if (_rectTransform) _rectTransform.localScale = _orgScale * _zoom;
    }

    private void OnMouseExit()
    {
        if (_rectTransform) _rectTransform.localScale = _orgScale;
    }
}