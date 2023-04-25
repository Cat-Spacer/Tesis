using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalShield : MonoBehaviour, IElectric
{
    public GameObject scrollbarObject;
    public GameObject topCapObject;
    public GameObject bottomCapObject;
    public float sensitivity = 1f;

    
    private Vector3 scrollbarStartPosition;
    private float scrollbarLength;
    private bool isDragging = false;
    private float dragStartPosition;

    [SerializeField] bool _IsOn = false;

    void Start()
    {
        //_IsOn = false;
        scrollbarStartPosition = scrollbarObject.transform.position;
        scrollbarLength = topCapObject.transform.position.y - bottomCapObject.transform.position.y;
    }

    void Update()
    {
        if (!_IsOn)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            dragStartPosition = Input.mousePosition.y;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            for (int i = 0; i < 5; i++)
            { // repetir la detección de la posición del mouse 5 veces por fotograma
                float dragDelta = Input.mousePosition.y - dragStartPosition;
                float newPosition = scrollbarObject.transform.position.y + dragDelta * sensitivity / 5f;
                newPosition = Mathf.Clamp(newPosition, bottomCapObject.transform.position.y, topCapObject.transform.position.y);
                scrollbarObject.transform.position = new Vector3(scrollbarStartPosition.x, newPosition, scrollbarStartPosition.z);
                dragStartPosition = Input.mousePosition.y;
            }
        }
    }
    public void TurnOff()
    {
        _IsOn = false;
    }

    public void TurnOn()
    {
        _IsOn = true;
    }

   /* void Start()
    {
        scrollbarStartPosition = scrollbarObject.transform.position;
        scrollbarLength = topCapObject.transform.position.y - bottomCapObject.transform.position.y;
    }

    void Update()
    {
        float input = Input.GetAxis("Vertical");
        float newPosition = scrollbarObject.transform.position.y + input * sensitivity;
        newPosition = Mathf.Clamp(newPosition, bottomCapObject.transform.position.y, topCapObject.transform.position.y);
        scrollbarObject.transform.position = new Vector3(scrollbarStartPosition.x, newPosition, scrollbarStartPosition.z);
    }*/
}
