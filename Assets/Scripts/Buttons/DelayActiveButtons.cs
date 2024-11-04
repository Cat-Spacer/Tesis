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
        if (!_activeThis || _activeThis.GetComponent<IDelayer>() == null)
        {
            //_activeThis = GameManager.Instance.StaticScreens()[Random.Range(0, GameManager.instance.StaticScreens().Count)].gameObject;

            if (!_activeThis)
            {
                Destroy(this);
                return;
            }
        }
        if (_menuByGO) _menuName = _menuByGO.name;
    }

    /// <summary>
    /// Start DelayActivation Corrutine by scene number
    /// </summary>
    public void BTN_Start(int scene = -1)
    {
        //Check();
        _activeThis.SetActive(true);

        StartCoroutine(DelayActivation(_menuName, scene));
    }

    /// <summary>
    /// Start DelayActivation Corrutine by screen name
    /// </summary>
    public void BTN_Start(string screen = " ")
    {
        //Check();
        if (_activeThis) _activeThis.SetActive(true);

        StartCoroutine(DelayActivation(screen, -1));
    }

    /// <summary>
    /// Start DelayActivation Corrutine by default
    /// </summary>
    public void BTN_Start()
    {
        //Check();
        _activeThis.SetActive(true);

        StartCoroutine(DelayActivation());
    }

    /// <summary>
    /// Start DelayActivation Corrutine by default
    /// </summary>
    public void BTN_Back()
    {
        //Check();
        _activeThis.SetActive(true);

        StartCoroutine(DelayActivation());
    }

    /// <summary>
    /// Active/Desactive game object with a delay then opens a scene or screen
    /// </summary>
    private IEnumerator DelayActivation(string menuName, int scene = -1)
    {
        yield return new WaitForSeconds(_delay);
        _activeThis.SetActive(false);        

        //if (MenuButton.instance)
        //    if (scene >= 0) MenuButton.instance.SceneToLoad(scene);
        //    else MenuButton.instance.BTN_General(menuName);

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
        //if (MenuButton.instance) MenuButton.instance.BTN_Back();
    }
}