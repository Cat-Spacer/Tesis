using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // [SerializeField] private GameObject _on, _off;
    // [SerializeField] private SoundManager.Types _sound = SoundManager.Types.Item;
    // private bool alreadyOn;
    //
    // private void Start()
    // {
    //     _on.SetActive(false);
    //     _off.SetActive(true);
    // }
    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject == GameManager.Instance.GetPlayer() && !alreadyOn)
    //     {
    //         SoundManager.instance.Play(_sound, false);
    //         GameManager.Instance.SetNewCheckPoint(transform);
    //         _on.SetActive(true);
    //         _off.SetActive(false);
    //         alreadyOn = true;
    //     }
    // }
}
