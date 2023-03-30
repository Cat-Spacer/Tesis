using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBirdCageFollow : MonoBehaviour
{
    public Transform player;
    Vector2 originalPos;
    public Vector3 offset;
    public Vector2 cameraRange;
    public float smoothFactor;
    public LayerMask playerLayerMask;
    public Collider2D coll;

    private void Start()
    {
        originalPos = player.position + offset;
    }
    void Update()
    {
        Follow();
    }
    void Follow()
    {
        coll = Physics2D.OverlapBox(transform.position, cameraRange, 0, playerLayerMask);
        if (!coll && Vector3.Distance(originalPos, transform.position) > 0.1)
        {
            Vector3 targetPosition = player.position + offset;
            Vector3 smoothPos = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.deltaTime);
            transform.position = smoothPos;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, cameraRange);
    }
}
