using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClestialDiamond : MonoBehaviour
{
    [SerializeField] ColliderSides _colliderArea;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.celestialDiamond = true;
        SoundManager.instance.Play(SoundManager.Types.ClestialDiamond);
        _colliderArea.ForceMove();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameManager.Instance.celestialDiamond = true;
            SoundManager.instance.Play(SoundManager.Types.ClestialDiamond);
            _colliderArea.ForceMove();
        }
    }
}
