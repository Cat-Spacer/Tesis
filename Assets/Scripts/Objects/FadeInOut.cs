using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField, Range(1.0f, 50.0f)] float _fadeSpeed = 1.0f;
    [SerializeField] bool _fadeOut = false, _fadeIn = false;
    [SerializeField, Range(0.75f, 1.0f)] float _maxFadeIn = 1.0f;
    [SerializeField, Range(0.0f, 0.6f)] float _maxFadeOut = 0.0f;

    private SpriteRenderer _spriteRenderer;
    private Color _color;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _color = _spriteRenderer.color;
        _spriteRenderer.color = _color;
    }
    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            StartCoroutine(FadeEffect());
    }*/

    public void StartFades(bool fade, GameObject target = null)
    {
        StartCoroutine(FadeEffect(fade, target));
    }

    IEnumerator FadeEffect(bool fade, GameObject target = null)
    {


        while (fade && _fadeOut)///Fade Out - screen on
        {
            if (target)
                target.SetActive(fade);
            Debug.Log($"_color.a = {_color.a}");
            _color.a -= Time.deltaTime / _fadeSpeed;
            _spriteRenderer.color = _color;
            if (_color.a < _maxFadeOut)
            {
                _fadeOut = false;
                _fadeIn = true;
            }
            yield return null;
        }

        while (!fade && _fadeIn)///Fade In - screen off
        {
            _color.a += Time.deltaTime / _fadeSpeed;
            _spriteRenderer.color = _color;
            if (target && _color.a >= _maxFadeIn)
            {
                target.SetActive(fade);
                _fadeOut = true;
                _fadeIn = false;
            }
            yield return null;
        }
    }
}