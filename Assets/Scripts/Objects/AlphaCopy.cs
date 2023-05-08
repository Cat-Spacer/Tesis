using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaCopy : MonoBehaviour
{
    [SerializeField] private Image _objetive = null;
    [SerializeField] private SpriteRenderer _target = null;
    

    void Update()
    {
        if (!(_objetive || _target)) return;
        var color = _objetive.color;
        color.a = 1 - _target.color.a;
        _objetive.color = color;
    }
}
