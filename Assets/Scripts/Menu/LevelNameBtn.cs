using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelNameBtn : MonoBehaviour
{
    [SerializeField] private LevelMenu lvlMenu;
    private Button _btn;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private bool _useAutomatic;   
    private void Awake()
    {
        if (!_useAutomatic) return;
        //if (_text)
        //{
        //    _text = GetComponentInChildren<TextMeshProUGUI>();
        //    _text.text = gameObject.name;
        //}
    }

    private void Start()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(SelectLevel);
    }

    void SelectLevel()
    {
        lvlMenu.SelectLevel(_btn.gameObject.name);
    }
}
