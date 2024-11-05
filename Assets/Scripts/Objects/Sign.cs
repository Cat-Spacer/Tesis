using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour,IInteract
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite signSprite;
    [SerializeField] Sprite happySignSprite;

    private bool _wasDrawed = false;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = signSprite;
    }

    void Draw()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        _wasDrawed = true;
        spriteRenderer.sprite = happySignSprite;
        if(EventManager.Instance != null)EventManager.Instance.Trigger(EventType.OnChangePeace, 1);
    }
    
    public void Interact(params object[] param)
    {
        if (_wasDrawed) return;
        Draw();
    }

    public void ShowInteract(bool showInteractState)
    {

    }
    private void OnValidate()
    {
        if (signSprite == null || spriteRenderer == null) return;
        spriteRenderer.sprite = signSprite;
    }
}
