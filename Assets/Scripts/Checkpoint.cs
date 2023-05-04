using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] GameObject _on, _off;
    bool alreadyOn;
    private void Start()
    {
        _on.SetActive(false);
        _off.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Instance.GetPlayer() && !alreadyOn)
        {
            GameManager.Instance.SetNewCheckPoint(transform);
            _on.SetActive(true);
            _off.SetActive(false);
            alreadyOn = true;
        }
    }
}
