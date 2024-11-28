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
    private void Start()
    {
        _button = GetComponent<Button>();
        _originalScale = transform.localScale;
    }

    bool onPointer = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointer = true;
        if (_button.transform.localScale == _originalScale)
            _button.transform.localScale *= _amount;
        if (SoundManager.instance != null) SoundManager.instance.Play(SoundsTypes.ButtonHover);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        onPointer = false;
        _button.transform.localScale = _originalScale;
    }

    private IEnumerator ScaleSizeLoop()
    {
        while (true)
        {
            while (onPointer)
            {
                if (_button.transform.localScale == _originalScale)
                    _button.transform.localScale *= _amount;

                yield return null;
            }

            _button.transform.localScale = _originalScale * _amount;

            yield return new WaitForSeconds(0.2f);

            while (onPointer)
            {
                if (_button.transform.localScale == _originalScale)
                    _button.transform.localScale *= _amount;

                yield return null;
            }

            _button.transform.localScale = _originalScale;

            yield return new WaitForSeconds(0.2f);
        }
    }
    public void CallSizeUpdate()
    {
        _scaleCoroutine = StartCoroutine(ScaleSizeLoop());
    }
    public void UncallSizeUpdate()
    {
        StopCoroutine(_scaleCoroutine);
        _button.transform.localScale = _originalScale;
    }
}