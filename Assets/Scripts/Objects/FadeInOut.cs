using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] float _fadeSpeed = 1.0f;
    [SerializeField] bool _fadeOut = false, _fadeIn = false;
    [SerializeField, Range(0.75f, 1.0f)] float _maxFadeIn = 1.0f;
    [SerializeField, Range(0.0f, 0.6f)] float _maxFadeOut = 0.0f;
    SpriteRenderer _spriteRenderer;
    Color _color;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ///_fadeSpeed = Mathf.Clamp(_fadeSpeed, _maxFadeOut, _maxFadeIn);
        _color = _spriteRenderer.color;
        _spriteRenderer.color = _color;
    }
    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            StartCoroutine(FadeEffect());
    }*/

    public void StartFades(bool fade)
    {
        StartCoroutine(FadeEffect(fade));
    }

    IEnumerator FadeEffect(bool fade)
    {
        /*if (_color.a >= _maxFadeIn)
        {
            _fadeOut = true;
            _fadeIn = false;
        }
        else if (_color.a < _maxFadeOut)
        {
            _fadeOut = false;
            _fadeIn = true;
        }*/

        while (fade)
        {
            _color.a -= Time.deltaTime / _fadeSpeed;
            _spriteRenderer.color = _color;
            yield return null;
        }

        while (!fade)
        {
            _color.a += Time.deltaTime / _fadeSpeed;
            _spriteRenderer.color = _color;
            yield return null;
        }
    }
}
