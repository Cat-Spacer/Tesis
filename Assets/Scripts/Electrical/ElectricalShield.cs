using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElectricalShield : MonoBehaviour, IElectric
{
    public Button scrollbarObject;
    public GameObject topCapObject;
    public GameObject bottomCapObject;
    public float sensitivity = 1f;

    
    private Vector3 scrollbarStartPosition;
    private float scrollbarLength;
    private bool isDragging = false;
    private float dragStartPosition;

    Transform _startPos;

    [SerializeField] bool _IsOn = false;

    [SerializeField] bool _inX = true;

    void Start()
    {
        _startPos = scrollbarObject.transform;
        EventManager.Instance.Subscribe("PlayerDeath", ResetPosition);
        //_IsOn = false;
        scrollbarStartPosition = scrollbarObject.transform.position;
        scrollbarLength = topCapObject.transform.position.y - bottomCapObject.transform.position.y;

    }
    void Update()
    {
       

        if (isDragging)
        {
            Debug.Log("is dragging 2");
            dragStartPosition = Input.mousePosition.y;
            for (int i = 0; i < 5; i++)
            { // repetir la detección de la posición del mouse 5 veces por fotograma
                float dragDelta = Input.mousePosition.y - dragStartPosition;
                float newPosition = scrollbarObject.transform.position.y + dragDelta * sensitivity / 5f;
                newPosition = Mathf.Clamp(newPosition, bottomCapObject.transform.position.y, topCapObject.transform.position.y);
                Debug.Log(newPosition);
                scrollbarObject.transform.position = new Vector3(scrollbarStartPosition.x, newPosition, scrollbarStartPosition.z);
                dragStartPosition = Input.mousePosition.y;
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
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
    public void Clicked()
    {
        if (!_IsOn ) return;
        Vector3 mousePosition = Input.mousePosition;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, -Camera.main.transform.position.z));

        //  Debug.Log(mousePosition + " " + worldPosition);

        if (!_inX )
        {
            if (worldPosition.y <= bottomCapObject.transform.position.y || worldPosition.y >= topCapObject.transform.position.y)
            {
                Debug.Log("RETURN");
                return;
            }
            else
            {

                scrollbarObject.transform.position = new Vector3(scrollbarObject.transform.position.x, worldPosition.y, scrollbarObject.transform.position.z);

                Debug.Log(" no RETURN " + scrollbarObject.transform.position);
            }
        }
        if (_inX)
        {
            if (worldPosition.x <= bottomCapObject.transform.position.x || worldPosition.x >= topCapObject.transform.position.x)
            {
                Debug.Log("RETURN");
                return;
            }
            else
            {
                scrollbarObject.transform.position = new Vector3(worldPosition.x, scrollbarObject.transform.position.y, scrollbarObject.transform.position.z);

                Debug.Log(" no RETURN " + scrollbarObject.transform.position);
            }
            
        }


        //isDragging = true;
    }
    void ResetPosition(params object[] param)
    {
        scrollbarObject.transform.position = scrollbarStartPosition;
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
