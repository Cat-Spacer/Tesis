using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSizeUpdate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _amount = 1.25f;
    private Button _button = default;
    private Vector3 _originalScale = default;

    private void Start()
    {
        _button = GetComponent<Button>();
        _originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _button.transform.localScale = _button.transform.localScale * _amount;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _button.transform.localScale = _originalScale;
    }
}