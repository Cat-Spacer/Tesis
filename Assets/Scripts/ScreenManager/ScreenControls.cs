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
        ScreenManager.Instance.Pop(false);
    }

    public void BTN_Exit()
    {
        Free();
    }

    public void Activate()
    {
        
    }

    public void Deactivate()
    {
        
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