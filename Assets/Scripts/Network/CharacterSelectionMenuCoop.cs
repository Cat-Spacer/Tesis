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
    [SerializeField] private GameObject menu;

    private bool inputState;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        _catTextPos.transform.position = _p1SelectedPlayer.transform.position;
        _hamsterTextPos.transform.position = _p2SelectedPlayer.transform.position;
        menu.SetActive(true);
        if (SoundManager.instance)
        {
            SoundManager.instance.PauseAll();
            SoundManager.instance.Play(SoundsTypes.Music, null, true);
        }
        if (GameManager.Instance) GameManager.Instance.pause = true;
    }
    public void SwitchCharacter()
    {
        if (SoundManager.instance) SoundManager.instance.Play(SoundsTypes.Button);
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
        if (SoundManager.instance) SoundManager.instance.Play(SoundsTypes.Button);
        if (player1 == CharacterType.Cat)
        {
            GameManager.Instance.StartGame(player1Inputs, player2Inputs);
        }
        else GameManager.Instance.StartGame(player2Inputs, player1Inputs);
        if (GameManager.Instance) GameManager.Instance.pause = false;
        gameObject.SetActive(false);
    }
    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        if (SoundManager.instance)
        {
            SoundManager.instance.ResetGOList();
            SoundManager.instance.Play(SoundsTypes.Button);
        }
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    private void OnEnable()
    {
        inputState = true;
    }
}
