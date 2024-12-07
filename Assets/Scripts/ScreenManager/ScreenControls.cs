using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class ScreenControls : ScreenBase, IScreen
{
    [SerializeField] private GameObject _alternativeBTN = default;

    private void Awake()
    {
        OnAwake();
        SetButton();
    }
    public void BTN_Back()
    {
        ScreenManager.instance.Pop(false);
    }

    public void BTN_Exit()
    {
        Free();
    }

    public void Activate()
    {
        if (buttons == null || buttons.Length <= 0) return;

        foreach (var button in buttons)
        {
            button.interactable = true;
        }
    }

    public void Deactivate()
    {
        if (buttons == null || buttons.Length <= 0) return;

        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }

    private void SetButton()
    {
        if (SceneManager.GetActiveScene().buildIndex > 2)
        {
            _alternativeBTN.SetActive(false);
            _alternativeBTN.SetActive(true);
        }
    }
}