using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class CharacterSelectionMenuCoop : MonoBehaviour
{
    public static CharacterSelectionMenuCoop Instance;

    [SerializeField] private Transform _p1SelectedPlayer;
    [SerializeField] private Transform _p2SelectedPlayer;
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
        _catTextPos.transform.position = _p1SelectedPlayer.transform.position;
        _hamsterTextPos.transform.position = _p2SelectedPlayer.transform.position;
    }
    public void SwitchCharacter()
    {
        SoundManager.instance.Play(SoundsTypes.Button);
        if (player1 == CharacterType.Cat)
        {
            player1 = CharacterType.Hamster;
            player2 = CharacterType.Cat;
            _p1SelectedPlayer.transform.position = _hamsterTextPos.transform.position;
            _p2SelectedPlayer.transform.position = _catTextPos.transform.position;
        }
        else
        {
            player1 = CharacterType.Cat;
            player2 = CharacterType.Hamster;
            _p1SelectedPlayer.transform.position = _catTextPos.transform.position;
            _p2SelectedPlayer.transform.position = _hamsterTextPos.transform.position;
        }
    }
    public void SelectCharacters()
    {
        SoundManager.instance.Play(SoundsTypes.Button);
        if (player1 == CharacterType.Cat)
        {
            GameManager.Instance.StartGame(player1Inputs, player2Inputs);
        }
        else GameManager.Instance.StartGame(player2Inputs, player1Inputs);
        gameObject.SetActive(false);
    }
    public void ReturnToMenu()
    {
        SoundManager.instance.Play(SoundsTypes.Button);
        Time.timeScale = 1;
        SoundManager.instance.Play(SoundsTypes.Button);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    private void OnEnable()
    {
        inputState = true;
    }
}
