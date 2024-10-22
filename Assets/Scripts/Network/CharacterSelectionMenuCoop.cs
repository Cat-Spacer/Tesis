using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionMenuCoop : MonoBehaviour
{
    public static CharacterSelectionMenuCoop Instance;

    [SerializeField] private Transform _player1SelectedText, _player2SelectedText;
    [SerializeField] Transform _catTextPos, _hamsterTextPos;
    
    
    [SerializeField] private CharacterType player1 = CharacterType.Cat;
    [SerializeField] private CharacterType player2 = CharacterType.Hamster;
    [SerializeField] private SO_Inputs player1Inputs;
    [SerializeField] private SO_Inputs player2Inputs;
    private Dictionary<CharacterType, SO_Inputs> playerInputDictionary = new Dictionary<CharacterType, SO_Inputs>();

    private bool inputState;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    
    private void Start()
    {
        _catTextPos.transform.position = _player1SelectedText.transform.position;
        _hamsterTextPos.transform.position = _player2SelectedText.transform.position;
    }

    private void Update()
    {
        if (inputState && Input.GetKeyDown(KeyCode.Space))
        {
            SwitchCharacter();
        }
    }

    public void SwitchCharacter()
    {
        if (player1 == CharacterType.Cat)
        {
            player1 = CharacterType.Hamster;
            player2 = CharacterType.Cat;
            _player1SelectedText.transform.position = _hamsterTextPos.transform.position;
            _player2SelectedText.transform.position = _catTextPos.transform.position;
        }
        else
        {
            player1 = CharacterType.Cat;
            player2 = CharacterType.Hamster;
            _player1SelectedText.transform.position = _catTextPos.transform.position;
            _player2SelectedText.transform.position = _hamsterTextPos.transform.position;
        }
    }

    public void SelectCharacters()
    {
        if (player1 == CharacterType.Cat) GameManager.Instance.StartGame(player1Inputs, player2Inputs);
        else GameManager.Instance.StartGame(player2Inputs, player1Inputs);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        inputState = true;
    }
}
