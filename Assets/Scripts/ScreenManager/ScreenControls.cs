using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class ScreenControls : ScreenBase, IScreen
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
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
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