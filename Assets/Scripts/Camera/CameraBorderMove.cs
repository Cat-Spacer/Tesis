using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBorderMove : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float _edgeSize = 10.0f, _moveAmount = 10.0f;
    void Start()
    {

    }

    void Update()
    {
        MousePosImp();
    }

    private void MousePosImp()
    {
        if (Input.mousePosition.x > Screen.width - _edgeSize)
            transform.position = new Vector3(transform.position.x + _moveAmount * Time.deltaTime, transform.position.y);
        if (Input.mousePosition.x < _edgeSize)
            transform.position = new Vector3(transform.position.x - _moveAmount * Time.deltaTime, transform.position.y);
        if (Input.mousePosition.y > Screen.height - _edgeSize)
            transform.position = new Vector3(transform.position.x, transform.position.y + _moveAmount * Time.deltaTime);
        if (Input.mousePosition.y < _edgeSize)
            transform.position = new Vector3(transform.position.x, transform.position.y - _moveAmount * Time.deltaTime);
    }
}
