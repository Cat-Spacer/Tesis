using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelNameBtn : MonoBehaviour
{
    [SerializeField] private LevelMenu lvlMenu = default;
    private Button _btn = default;
    [SerializeField] private TextMeshProUGUI _text = default;
    [SerializeField] private GameObject _selectedBorder = default;
    [SerializeField] private ButtonSizeUpdate _buttonSizeUpdate;
    [SerializeField] private bool _useAutomatic = default;
    private bool _selected = default;

    public bool selected { get { return _selected; } }


    private void Awake()
    {
        if (!_useAutomatic || !_selectedBorder) return;
        
        if (_selectedBorder.activeInHierarchy) SetSelected(false);
    }

    private void Start()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(SelectLevel);
    }

    private void SelectLevel()
    {
        lvlMenu.SelectLevel(_btn.gameObject.name);
        lvlMenu.StaySelected(this);
    }

    public void SetSelected(bool selected)
    {
        _selected = selected;
        if (_selectedBorder) _selectedBorder.SetActive(selected);
        if (_buttonSizeUpdate && selected) _buttonSizeUpdate.CallSizeUpdate();
        else if (_buttonSizeUpdate && !selected) _buttonSizeUpdate.UncallSizeUpdate();
    }

}