using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBirdCageFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float smoothFactor;

    void Update()
    {
        Follow();
    }
    void Follow()
    {
        Vector3 targetPosition = player.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.deltaTime);
        transform.position = smoothPos;
    }
}
