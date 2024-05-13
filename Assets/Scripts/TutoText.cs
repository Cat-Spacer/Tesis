using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoText : MonoBehaviour
{
    // [SerializeField] private DialogueTrigger _dialogue;
    // [SerializeField] private GameObject _icon;
    // [SerializeField] private Sprite _on, _off;
    // [SerializeField] private SoundManager.Types _sound = SoundManager.Types.Item;
    // private SpriteRenderer _sp;
    //
    // void Start()
    // {
    //     _sp = GetComponent<SpriteRenderer>();
    //     _dialogue = GetComponent<DialogueTrigger>();
    //     _icon.SetActive(false);
    //     _sp.sprite = _off;
    // }
    //
    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.GetComponent<CustomMovement>())
    //     {
    //         SoundManager.instance.Play(_sound, false);
    //         _dialogue.TriggerDialogue();
    //         _icon.SetActive(true);
    //         _sp.sprite = _on;
    //     }
    // }
    //
    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (collision.gameObject.GetComponent<CustomMovement>())
    //     {
    //         //SoundManager.instance.Play(_sound, false);
    //         _dialogue.EndDialogue();
    //         _icon.SetActive(false);
    //         _sp.sprite = _off;
    //     }
    // }
}