using System;
using System.Collections;
using UnityEngine;

public class DelayActiveButtons : MonoBehaviour
{
    [SerializeField] private GameObject _sceneByGO = default, _activeThis = default;
    [SerializeField] private GameObject[] _desactive = default;
    [SerializeField] private float _delay = 1.0f;
    [SerializeField] private string _menuName = default;
    //[SerializeField] private int loadingScreenIndex = 0;


    void Start()
    {
        if (!_activeThis) Destroy(this);
        if (_sceneByGO) _menuName = _sceneByGO.name;
    }

    public void BTN_Start(int scene = -1)
    {

        foreach (var item in _desactive)
        {
            item.SetActive(false);
        }
        _activeThis.SetActive(true);

        StartCoroutine(DelayActivation(_menuName, scene));
    }

    private IEnumerator DelayActivation(string menuName, int scene = -1)
    {
        yield return new WaitForSeconds(_delay);
        _activeThis.SetActive(false);

        if (MenuButton.instance)
            if (scene >= 0) MenuButton.instance.SceneToLoad(scene);
            else MenuButton.instance.BTN_General(menuName);
    }
}