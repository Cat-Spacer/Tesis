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
        if (!_tube) _tube = GetComponentInParent<Tube>();
        DisableEnableButton();
    }

    public void DisableEnableButton()
    {
        if (!_tube)
        {
            gameObject.SetActive(false);
            return;
        }

        if ((_tubeActions == TubeActions.Up && !_tube.GetUp()) ||
            (_tubeActions == TubeActions.Down && !_tube.GetDown()) ||
            (_tubeActions == TubeActions.Left && !_tube.GetLeft()) ||
            (_tubeActions == TubeActions.Right && !_tube.GetRight()))
            gameObject.SetActive(false);

        if ((_tubeActions == TubeActions.Up && _tube.GetUp()) ||
            (_tubeActions == TubeActions.Down && _tube.GetDown()) ||
            (_tubeActions == TubeActions.Left && _tube.GetLeft()) ||
            (_tubeActions == TubeActions.Right && _tube.GetRight()))
            gameObject.SetActive(true);
    }

    private void EjecuteOrder66()
    {
        if (!_tube) return;

        var button = GetComponent<Button>();
        var ped = new PointerEventData(EventSystem.current);

        if (!button) return;
        ExecuteEvents.Execute(button.gameObject, ped, ExecuteEvents.pointerEnterHandler);
        ExecuteEvents.Execute(button.gameObject, ped, ExecuteEvents.submitHandler);
    }

    public void GoToTube()
    {
        if (_tubeActions == TubeActions.Up && _tube) _tube.GoUp();
        if (_tubeActions == TubeActions.Down && _tube) _tube.GoDown();
        if (_tubeActions == TubeActions.Left && _tube) _tube.GoLeft();
        if (_tubeActions == TubeActions.Right && _tube) _tube.GoRight();
        gameObject.SetActive(false);
    }

    public Tube Tube { get { return _tube; } set { _tube = value; } }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            EjecuteOrder66();
    }
}