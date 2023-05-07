using UnityEngine;

public class CameraFade : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private KeyCode _key = KeyCode.Space;
    [SerializeField] private float _speedScale = 1f;
    [SerializeField] private Color _fadeColor = Color.black;
    [SerializeField]
    private AnimationCurve _curve = new AnimationCurve(new Keyframe(0, 1),
        new Keyframe(0.5f, 0.5f, -1.5f, -1.5f), new Keyframe(1, 0));
    [SerializeField] private bool _startFadedOut = false, _followCamera = false;
    [SerializeField] private SpriteRenderer _spriteRenderer = null;

    private Texture2D _texture;
    private float _alpha = 0f;
    private int _direction = 0;
    private float _time = 0f;

    private void Start()
    {
        //_camera = GetComponent<Camera>();
        //Debug.Log($"_camera.scaledPixelWidth = {_camera.scaledPixelWidth}, _camera.scaledPixelHeight = {_camera.scaledPixelHeight}");
        if (_startFadedOut) _alpha = 1f; else _alpha = 0f;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (!_spriteRenderer)
        {
            _texture = new Texture2D(1, 1);
            _texture.SetPixel(0, 0, new Color(_fadeColor.r, _fadeColor.g, _fadeColor.b, _alpha));
            _texture.Apply();
        }
    }

    private void Update()
    {
        FollowObject();
        if (_direction == 0 && Input.GetKeyDown(_key))
        {
            if (_alpha >= 1f) // Fully faded out
            {
                _alpha = 1f;
                _time = 0f;
                _direction = 1;
            }
            else // Fully faded in
            {
                _alpha = 0f;
                _time = 1f;
                _direction = -1;
            }
            FadeTexture();
        }
    }

    private void FollowObject()
    {
        if (_followCamera && _target)
        {
            transform.position = (Vector2)_target.transform.position;
        }
    }

    private void FadeTexture()
    {
        if (_direction != 0)
        {
            if (!_spriteRenderer)
            {
                _time += _direction * Time.deltaTime * _speedScale;
                _alpha = _curve.Evaluate(_time);
                _texture.SetPixel(0, 0, new Color(_fadeColor.r, _fadeColor.g, _fadeColor.b, _alpha));
                _texture.Apply();
                if (_alpha <= 0f || _alpha >= 1f) _direction = 0;
            }
            else
            {
                _time += _direction * Time.deltaTime * _speedScale;
                var color = _spriteRenderer.color;
                color.a = _curve.Evaluate(_time);
                _spriteRenderer.color = color;
                _spriteRenderer.color = new Color(_fadeColor.r, _fadeColor.g, _fadeColor.b, color.a);
                if (_alpha <= 0f || _alpha >= 1f) _direction = 0;
            }
        }
    }

    public void OnGUI()
    {
        //if (_alpha > 0f && _camera && !_followCamera) GUI.DrawTexture(new Rect(0, 0, _camera.scaledPixelWidth, _camera.scaledPixelHeight), _texture);
        FadeTexture();
    }
}