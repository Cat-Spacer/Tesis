using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonSizeUpdate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _amount = 1.25f;
    private Button _button = default;
    private Vector3 _originalScale = default;
    private Coroutine _scaleCoroutine;
    [SerializeField]private Color _selectedColor;
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _highlitedColor;
    [SerializeField] private Image _image;
    private bool _changeColor = false;
    public Vector3 GetOriginalScale { get { return _originalScale; } }
    private void Start()
    {
        if (_image != null)
            _changeColor = true;

        _button = GetComponent<Button>();
        _originalScale = transform.localScale;
    }

    private bool _onPointer = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
           _onPointer = true;
        if (_changeColor && !_isCliked)
            _image.color = _highlitedColor;
        if (_button.transform.localScale == _originalScale)
            _button.transform.localScale *= _amount;
        if (SoundManager.instance != null) SoundManager.instance.Play(SoundsTypes.ButtonHover);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _onPointer = false;
        if (_changeColor && !_isCliked)
            _image.color = _normalColor;
        _button.transform.localScale = _originalScale;
    }

    private IEnumerator ScaleSizeLoop()
    {
        while (true)
        {
            while (_onPointer)
            {
                if (_button.transform.localScale == _originalScale)
                    _button.transform.localScale *= _amount;

                yield return null;
            }

            _button.transform.localScale = _originalScale * _amount;

            yield return new WaitForSeconds(0.2f);

            while (_onPointer)
            {
                if (_button.transform.localScale == _originalScale)
                    _button.transform.localScale *= _amount;

                yield return null;
            }

            _button.transform.localScale = _originalScale;

            yield return new WaitForSeconds(0.2f);
        }
    }
    
    private bool _isCliked = false;
    public void CallSizeUpdate()
    {
         if (_changeColor)
            _image.color = _selectedColor;
         _isCliked = true;
         _scaleCoroutine = StartCoroutine(ScaleSizeLoop());
    }
    public void UncallSizeUpdate()
    {
        if (_changeColor) 
            _image.color = _normalColor;
        StopCoroutine(_scaleCoroutine);
        _isCliked = false;
        _button.transform.localScale = _originalScale;
    }
}