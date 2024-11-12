using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderSizeAdjust : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Camera camera;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        float screenHeight = camera.orthographicSize * 2f;
        float screenWidth = screenHeight * camera.aspect;
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        float scaleX = screenWidth / spriteSize.x;
        float scaleY = screenHeight / spriteSize.y;
        transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
}
