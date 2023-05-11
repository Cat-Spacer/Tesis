using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BubbleMapButton : MonoBehaviour
{
    private void ButtonAction()
    {
        var button = GetComponent<Button>();
        var ped = new PointerEventData(EventSystem.current);

        if (!button) return;
        ExecuteEvents.Execute(button.gameObject, ped, ExecuteEvents.pointerEnterHandler);
        ExecuteEvents.Execute(button.gameObject, ped, ExecuteEvents.submitHandler);
    }

    private void OnMouseOver()
    {
        //Debug.Log($"mouse is over {gameObject.name} of {gameObject.transform.parent.name}");
        if (Input.GetKeyDown(KeyCode.Mouse0))
            ButtonAction();
    }
}