using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoText : MonoBehaviour
{
    [SerializeField] DialogueTrigger _dialogue;
    [SerializeField] GameObject _icon;
    [SerializeField] private Sprite _on, _off;
    private SpriteRenderer _sp;

    void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
        _dialogue = GetComponent<DialogueTrigger>();
        _icon.SetActive(false);
        _sp.sprite = _off;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CustomMovement>())
        {
            _dialogue.TriggerDialogue();
            _icon.SetActive(true);
            _sp.sprite = _on;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CustomMovement>())
        {
            _dialogue.EndDialogue();
            _icon.SetActive(false);
            _sp.sprite = _off;
        }
    }
}