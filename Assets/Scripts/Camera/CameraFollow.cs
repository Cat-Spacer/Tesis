using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    void CheckBounds()
    {
        if (transform.position.y > 5) transform.position = new Vector2(transform.position.x, transform.position.y);
        if (transform.position.y < -5) transform.position = new Vector2(transform.position.x, transform.position.y);
        if (transform.position.x < -5) transform.position = new Vector2(5, transform.position.y);
        if (transform.position.x > 5) transform.position = new Vector2(-5, transform.position.y);
    }


    //get player position when colliding and - to collider. collider - player pos. if is positive then move to right. if is negative then move to left.
}
