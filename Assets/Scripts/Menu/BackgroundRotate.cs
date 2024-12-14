using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRotate : MonoBehaviour
{
    [SerializeField] private float speed;
    void Update()
    {
        var currentSpeed = speed * Time.deltaTime;
        transform.Rotate(0, 0, currentSpeed);
    }
}
