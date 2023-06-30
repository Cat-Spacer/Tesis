using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HamsterArrows : MonoBehaviour
{
    [SerializeField] private Tube _tube;
    [SerializeField] private Hamster _hamster;
    [SerializeField] private ArrowMoveButton[] _arrowMoveButton;

    void Start()
    {
        if (!_hamster) _hamster = GetComponentInParent<Hamster>();
        if (_hamster && !_tube) _tube = _hamster.CurrentTube;
    }

    private void Update()
    {
        if (!_hamster && _arrowMoveButton.Length < 1) if(!_hamster.InTube()) return;
        HelloThere();
    }

    private void HelloThere()
    {
        if (_hamster && !_tube) _tube = _hamster.CurrentTube;
        foreach (var tube in _arrowMoveButton)
        {
            if (tube) tube.Tube = _tube;
        }
    }
}