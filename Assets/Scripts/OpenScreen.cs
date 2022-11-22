using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScreen : MonoBehaviour
{
    [SerializeField] GameObject _screen;
    bool _started = false;
    [SerializeField] bool _desactivateAutomatic = false;
    [SerializeField] float _time = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_started) return;

        var player = collision.GetComponent<CustomMovement>();
        if (player == null) return;
        _screen.SetActive(true);
        if (_desactivateAutomatic)
            StartCoroutine(EndCoroutine());
            _started = true;

       
    }
    IEnumerator EndCoroutine()
    {
        yield return new WaitForSeconds(_time);
        _screen.SetActive(false);
    }
}
