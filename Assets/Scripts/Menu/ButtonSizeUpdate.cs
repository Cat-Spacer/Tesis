using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSizeUpdate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Button _button;
    [SerializeField] float amount;
    Vector3 originalScale;
    private void Start()
    {
        _button = GetComponent<Button>();
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _button.transform.localScale = _button.transform.localScale * amount;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _button.transform.localScale = originalScale;
    }
}
