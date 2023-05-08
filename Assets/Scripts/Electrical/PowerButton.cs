using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum GeneratorActions
{
    TurnOff,
    TurnOn,
}

public class PowerButton : MonoBehaviour
{
    [SerializeField] private Generator _generator;
    [SerializeField] private GeneratorActions _generatorActions;

    void Start()
    {
        if (!_generator)
            _generator = GetComponentInParent<Generator>();
    }

    private void EjecuteOrder66()
    {
        if (!_generator) return;
        var button = GetComponent<Button>();
        //Debug.Log($"{gameObject.name}: _generatorActions = {_generatorActions}, button = {button}");
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