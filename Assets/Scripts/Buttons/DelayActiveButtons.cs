using System.Collections;
using UnityEngine;

public class DelayActiveButtons : MonoBehaviour
{
    [SerializeField] private GameObject _menuByGO = default, _activeThis = default;
    [SerializeField] private GameObject[] _desactive = default, _active = default;
    [SerializeField] private float _delay = 1.0f;
    [SerializeField] private string _menuName = default;
    //[SerializeField] private int loadingScreenIndex = 0;


    void Start()
    {
        if (!_activeThis) Destroy(this);
        if (_menuByGO) _menuName = _menuByGO.name;
    }

    /// <summary>
    /// Activate/Desactive game objects, push menus & load scenes
    /// </summary>
    public void BTN_Start(int scene = -1)
    {
        _activeThis.SetActive(true);

        StartCoroutine(DelayActivation(_menuName, scene));
    }

    /// <summary>
    /// Activate/Desactive game objects
    /// </summary>
    public void BTN_Start()
    {
        _activeThis.SetActive(true);

        StartCoroutine(DelayActivation());
    }

    private IEnumerator DelayActivation(string menuName, int scene = -1)
    {
        yield return new WaitForSeconds(_delay);
        _activeThis.SetActive(false);        

        if (MenuButton.instance)
            if (scene >= 0) MenuButton.instance.SceneToLoad(scene);
            else MenuButton.instance.BTN_General(menuName);

        foreach (var item in _desactive)
        {
            item.SetActive(false);
        }
    }
    private IEnumerator DelayActivation()
    {
        yield return new WaitForSeconds(_delay);
        _activeThis.SetActive(false);

        foreach (var item in _active)
        {
            item.SetActive(true);
        }

        foreach (var item in _desactive)
        {
            item.SetActive(false);
        }
    }
}