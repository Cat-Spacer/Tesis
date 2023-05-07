using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraBorderMove : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float _edgeSize = 10.0f;
    [SerializeField, Range(0f, 20f)] private float _moveAmount = 10.0f;
    [SerializeField, Range(0f, 1f)] private float _smooth = 1.0f;
    void Start()
    {
        if (_moveAmount < _smooth)
        {
            float aux = _smooth;
            _smooth = _moveAmount;
            _moveAmount = aux;
        }
    }

    void Update()
    {
        MousePosImp();
    }

    private void MousePosImp()
    {
        var mov = Mathf.Lerp(_smooth, _moveAmount, Time.deltaTime);
        if (Input.mousePosition.x > Screen.width - _edgeSize)
            transform.position = new Vector3(transform.position.x + mov, transform.position.y, transform.position.z);
        if (Input.mousePosition.x < _edgeSize)
            transform.position = new Vector3(transform.position.x - mov, transform.position.y, transform.position.z);
        if (Input.mousePosition.y > Screen.height - _edgeSize)
            transform.position = new Vector3(transform.position.x, transform.position.y + mov, transform.position.z);
        if (Input.mousePosition.y < _edgeSize)
            transform.position = new Vector3(transform.position.x, transform.position.y - mov, transform.position.z);
    }
}
