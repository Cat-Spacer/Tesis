using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionMenuCoop : MonoBehaviour
{
    public static CharacterSelectionMenuCoop Instance = null;

    [SerializeField] private Transform _p1SelectedPlayer = default, _p2SelectedPlayer = default;
    [SerializeField] private CharacterType player1 = CharacterType.Cat, player2 = CharacterType.Hamster;
    [SerializeField] private SO_Inputs player1Inputs, player2Inputs;
    [SerializeField] private GameObject menu;

    private bool inputState;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
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
        if (!_p1SelectedPlayer || !_p2SelectedPlayer) return;
        if (player1 == CharacterType.Cat)
        {
            player1 = CharacterType.Hamster;
            player2 = CharacterType.Cat;
        }
        else
        {
            player1 = CharacterType.Cat;
            player2 = CharacterType.Hamster;
        }

        Vector2 posAux = _p1SelectedPlayer.position;
        _p1SelectedPlayer.position = _p2SelectedPlayer.transform.position;
        _p2SelectedPlayer.transform.position = posAux;
    }

    public void SelectCharacters()
    {
        if (!(GameManager.Instance || EventManager.Instance)) return;
        if (player1 == CharacterType.Cat)
        {
            GameManager.Instance.StartGame(player1Inputs, player2Inputs);
        }
        else GameManager.Instance.StartGame(player2Inputs, player1Inputs);

        GameManager.Instance.pause = false;
        GameManager.Instance.EnableByBehaviour();

        EventManager.Instance.Trigger(EventType.ReturnGameplay);
        gameObject.SetActive(false);
    }

    public void ReturnToMenu()
    {
        //Time.timeScale = 1;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    private void OnEnable()
    {
        inputState = true;
    }
}