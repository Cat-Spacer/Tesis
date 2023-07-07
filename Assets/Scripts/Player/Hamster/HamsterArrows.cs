using UnityEngine;
using static UnityEditor.UIElements.ToolbarMenu;

public class HamsterArrows : MonoBehaviour
{
    [SerializeField] private Tube _tube = null;
    [SerializeField] private Hamster _hamster = null;
    [SerializeField] private ArrowMoveButton[] _arrowMoveButton = new ArrowMoveButton[0];

    void Start()
    {
        if (!_hamster) _hamster = GetComponentInParent<Hamster>();
        if (_hamster && !_tube) _tube = _hamster.CurrentTube;
    }

    public void SetTubes()
    {
        if (_hamster && !_tube) _tube = _hamster.CurrentTube;
        foreach (var tube in _arrowMoveButton)
        {
            if (tube)
            {
                tube.Tube = _tube;
                tube.DisableEnableButton();
            }
        }
    }

    public void ResetTubes()
    {
        if (_hamster && _tube) _tube = null;
        foreach (var tube in _arrowMoveButton)
        {
            if (tube)
            {
                tube.Tube = null;
                tube.DisableEnableButton();
            }
        }
    }
}