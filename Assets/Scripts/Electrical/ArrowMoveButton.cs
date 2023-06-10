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
        DisableButton();
    }

    private void DisableButton()
    {
        if (!_tube) return;
        if ((_tubeActions == TubeActions.Up && !_tube.GetUp()) ||
            (_tubeActions == TubeActions.Down && !_tube.GetDown()) ||
            (_tubeActions == TubeActions.Left && !_tube.GetLeft()) ||
            (_tubeActions == TubeActions.Right && !_tube.GetRight()))
            gameObject.SetActive(false);
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
}