using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] Vector2 intensity;
    public Transform playerTransform;
    private Vector3 lastCameraPosition;

    private void Start()
    {
        lastCameraPosition = playerTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = playerTransform.position - lastCameraPosition;
        transform.position += new Vector3(-deltaMovement.x * intensity.x, -deltaMovement.y * intensity.y);
        lastCameraPosition = playerTransform.position;
    }
}
