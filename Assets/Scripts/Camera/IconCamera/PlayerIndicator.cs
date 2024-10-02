using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicator : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private RectTransform _indicator, _canvasRect, _arrow;
    [SerializeField] private RawImage _indicatorImage;

    void Start()
    {
        if (!_indicatorImage) _indicatorImage = _indicator.GetComponent<RawImage>();
        if (!_mainCamera) _mainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 screenPos = _mainCamera.WorldToScreenPoint(_player.position);
        bool isOffScreen = screenPos.x <= 0 || screenPos.x >= Screen.width || screenPos.y <= 0 || screenPos.y >= Screen.height;

        _indicatorImage.enabled = isOffScreen;

        if (isOffScreen)
        {
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, screenPos, null, out canvasPos);

            // Calcular la dirección desde el centro de la pantalla al jugador
            Vector2 dir = ((Vector2)_indicator.position + (Vector2)_player.position).normalized;

            // Calcular el ángulo de rotación para la flecha
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            _arrow.rotation = Quaternion.Euler(0, 0, angle - 90); // -90 para ajustar la orientación inicial de la flecha
        }
    }
}