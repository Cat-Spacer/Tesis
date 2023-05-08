using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum TubeActions
{
    None,
    Up,
    Down,
    Left,
    Right
}

public class ArrowMoveButton : MonoBehaviour
{
    [SerializeField] private Tube _tube;
    [SerializeField] private TubeActions _tubeActions;

    void Start()
    {
        if (!_tube)
            _tube = GetComponentInParent<Tube>();
    }

    private void EjecuteOrder66()
    {
        if (!_tube) return;
        var button = GetComponent<Button>();
        //Debug.Log($"{gameObject.name}: _tubeActions = {_tubeActions}, button = {button}");
        var ped = new PointerEventData(EventSystem.current);
        if (!button) return;
        ExecuteEvents.Execute(button.gameObject, ped, ExecuteEvents.pointerEnterHandler);
        ExecuteEvents.Execute(button.gameObject, ped, ExecuteEvents.submitHandler);
    }

    private void OnMouseOver()
    {
        //Debug.Log($"mouse is over {gameObject.name} of {gameObject.transform.parent.name}");
        if (Input.GetKeyDown(KeyCode.Mouse0))
            EjecuteOrder66();
    }

    /*private void OnMouseDown()
    {
        Debug.Log($"mouse is down {gameObject.name} of {gameObject.transform.parent.name}");
        if (Input.GetKeyDown(KeyCode.Mouse0))
            EjecuteOrder66();
    }*/
}