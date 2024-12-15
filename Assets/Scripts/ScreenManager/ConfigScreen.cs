using UnityEngine;

public class ConfigScreen : MonoBehaviour
{
    [SerializeField] private Transform _mainGame  = null;

    private void Start()
    {
        ScreenManager.Instance.Push(new ScreenGO(_mainGame));
    }
}
