using UnityEngine;

public class IconFollower : MonoBehaviour
{
    [SerializeField] private Transform _target = default; // The character to follow
    [SerializeField] private float _edgeBuffer = 10f, _smoothTime = 0.1f; // Time to reach the target

    private Vector2 _velocity = default;
    private Camera _mainCamera = default;
    private RectTransform _rectTransform = default, _canvasRect = default;
    [SerializeField] private GameObject[] _childrens = default;
    Vector2 _bounds = default;

    void Start()
    {
        //if (!_target) return;
        _mainCamera = Camera.main;
        _rectTransform = GetComponent<RectTransform>();
        _canvasRect = transform.parent.GetComponent<RectTransform>();
        if (_childrens.Length <= 0) _childrens = GetComponentsInChildren<GameObject>();
        foreach (GameObject child in _childrens) child.SetActive(false);
        _bounds = new(_canvasRect.rect.width / 2f, _canvasRect.rect.height / 2f);
    }

    void LateUpdate()
    {
        if (!_target) return;

        Vector2 targetViewportPosition = _mainCamera.WorldToViewportPoint(_target.position);

        if (targetViewportPosition.x < 0 || targetViewportPosition.x > 1 || targetViewportPosition.y < 0 || targetViewportPosition.y > 1)
        {
            foreach (GameObject child in _childrens) child.SetActive(true);

            Vector2 iconPosition = new(Mathf.Clamp(targetViewportPosition.x,  _edgeBuffer - 1, 1 - _edgeBuffer) * _bounds.x, Mathf.Clamp(targetViewportPosition.y, _edgeBuffer - 1, 1 - _edgeBuffer) * _bounds.y);

            _rectTransform.anchoredPosition = Vector2.SmoothDamp(_rectTransform.anchoredPosition, iconPosition, ref _velocity, _smoothTime);
        }
        else
        {
            foreach (GameObject child in _childrens) child.SetActive(false);
        }
    }
}