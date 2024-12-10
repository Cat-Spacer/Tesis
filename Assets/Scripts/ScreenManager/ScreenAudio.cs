using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class ScreenAudio : ScreenBase, IScreen
{
    public static ScreenAudio instance;
    [SerializeField] private GameObject _alternativeBTN = default, _currentBTN = default;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
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
            if (button != null)
                button.interactable = true;
        }
    }

    public void Deactivate()
    {
        foreach (var button in buttons)
        {
            if (button != null)
                button.interactable = false;
        }
    }

    private void SetButton()
    {
        if (SceneManager.GetActiveScene().buildIndex > 2 && _alternativeBTN && _currentBTN)
        {
            _currentBTN.SetActive(false);
            _alternativeBTN.SetActive(true);
        }
    }
}