using System;
using System.Collections;
using UnityEngine;

public class DelayActiveButtons : MonoBehaviour
{
    [SerializeField] private GameObject _activeThis = default;
    [SerializeField] private GameObject[] _desactive = default;
    [SerializeField] private float _delay = 1.0f;
    [SerializeField] private string _menuName = default;

    void Start()
    {
        if (!_activeThis) Destroy(this);       
    }

    public void BTN_Start()
    {
        foreach (var item in _desactive)
        {
            item.SetActive(false);
        }
        _activeThis.SetActive(true);

        if (MenuButton.instance) StartCoroutine(DelayActivation(_menuName));
    }

    private IEnumerator DelayActivation(string menuName)
    {
        yield return new WaitForSeconds(_delay);
        _activeThis.SetActive(false);
        MenuButton.instance.BTN_General(menuName);
    }
}