using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class ScreenVideo : ScreenBase, IScreen
{
    [SerializeField] private GameObject _alternativeBTN = default, _currentBTN = default;

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
        foreach (var button in buttons)
        {
            button.interactable = true;
        }
    }

    public void Deactivate()
    {
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