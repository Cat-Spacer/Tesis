using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameLevelMenu menu;
    private Respawn _respawnManager;
    [SerializeField] private PlayerCharacter _catPlayer, _hamsterPlayer;
    public Transform mainGame = null;
    [SerializeField] private string _level, _nextLevel;
    public bool pause = true;
    [SerializeField] private StartDoor[] _startDoors;
    [SerializeField] private float mouseIdleTimer;
    [SerializeField] private float checkInterval;
    private Vector3 _lastMousePosition;
    private bool _isMouseVisible = true, _onStopCursor;
    private Coroutine _mouseCoroutine;
    private Behaviour[] _behaviours = null;
    private readonly Dictionary<Behaviour, bool> _before = new();

    public CatCharacter SetCatCharacter
    {
        set { _catPlayer = value; }
    }

    public HamsterChar SetHamsterChar
    {
        set { _hamsterPlayer = value; }
    }

    private void Awake()
    {
        pause = true;
        if (Instance == null) Instance = this;
        _respawnManager = GetComponentInChildren<Respawn>();
        _lastMousePosition = Input.mousePosition;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        if (mainGame) _behaviours = mainGame.GetComponentsInChildren<Behaviour>();
    }

    private void Start()
    {
        GetLevels();
        _mouseCoroutine = StartCoroutine(CheckMouseMovement());
        if (SoundManager.instance)
        {
            SoundManager.instance.Play(SoundsTypes.Music, null, true);
        }
        if (mainGame) StartCoroutine(DisableByBehaviour(0.1f));
    }
    private void OnEnable()
    {
        if(EventManager.Instance == null) return;
        EventManager.Instance.Subscribe(EventType.OnLoseGame, OnLoseGame);
        EventManager.Instance.Subscribe(EventType.OnFinishGame, OnFinishGame);
        EventManager.Instance.Subscribe(EventType.OnResumeGame, OnResumeGame);
        EventManager.Instance.Subscribe(EventType.OnPauseGame, OnPauseGame);
    }
    private void OnPauseGame(object[] obj)
    {
        if (_mouseCoroutine != null) StopCoroutine(_mouseCoroutine);
        _mouseCoroutine = null;
        Cursor.visible = true;
        _onStopCursor = true;
        pause = true;
        if (mainGame) StartCoroutine(DisableByBehaviour());
    }

    private void OnResumeGame(object[] obj)
    {
        Cursor.visible = false;
        _onStopCursor = false;
        _lastMousePosition = Input.mousePosition;
        _mouseCoroutine = StartCoroutine(CheckMouseMovement());
        pause = false;
        EnableByBehaviour();
    }

    private void OnFinishGame(object[] obj)
    {
        if (_mouseCoroutine != null) StopCoroutine(_mouseCoroutine);
        _mouseCoroutine = null;
        Cursor.visible = true;
        _onStopCursor = true;
        if (mainGame) StartCoroutine(DisableByBehaviour());
    }

    private void OnLoseGame(object[] obj)
    {
        if (_mouseCoroutine != null) StopCoroutine(_mouseCoroutine);
        _mouseCoroutine = null;
        Cursor.visible = true;
        _onStopCursor = true;
        if (mainGame) StartCoroutine(DisableByBehaviour());
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.R)) EventManager.Instance.Trigger(EventType.OnLoseGame);
            if (Input.GetKeyDown(KeyCode.C)) KillPlayer(CharacterType.Cat);
            if (Input.GetKeyDown(KeyCode.H)) KillPlayer(CharacterType.Hamster);
            if(Input.GetKeyDown(KeyCode.W))
            {
                EventManager.Instance.Trigger(EventType.OnFinishGame);
                Invoke("CheatWin", 1);
            }
        }
    }

    void CheatWin()
    {
        EventManager.Instance.Trigger(EventType.ShowTv);
        WinLevel();
    }
    IEnumerator CheckMouseMovement()
    {
        float idleTimer = 0f;
        while (true)
        {
            if (Input.mousePosition != _lastMousePosition)
            {
                idleTimer = 0f;

                if (!_isMouseVisible)
                {
                    Cursor.visible = true;
                    _isMouseVisible = true;
                }
            }
            else
            {
                idleTimer += checkInterval;
                if (idleTimer >= mouseIdleTimer && _isMouseVisible)
                {
                    Cursor.visible = false;
                    _isMouseVisible = false;
                }
            }

            _lastMousePosition = Input.mousePosition;
            yield return new WaitForSeconds(checkInterval);
        }
    }

    void KillPlayer(CharacterType type)
    {
        if (type == CharacterType.Cat) _catPlayer.GetDamage();
        else _hamsterPlayer.GetDamage();
    }

    void GetLevels()
    {
        var currentLevel = SceneManager.GetActiveScene();
        _level = currentLevel.name;
        int nextLevelIndex = currentLevel.buildIndex + 1;
        if (nextLevelIndex < SceneManager.sceneCountInBuildSettings)
        {
            _nextLevel = SceneUtility.GetScenePathByBuildIndex(nextLevelIndex);
            _nextLevel = System.IO.Path.GetFileNameWithoutExtension(_nextLevel);
        }
        else _nextLevel = "No hay siguiente nivel"; // O manejar este caso como prefieras
    }

    public void WinLevel()
    {
        Cursor.visible = true;
        menu.WinMenu();
    }

    public void StartGame(SO_Inputs catInputs, SO_Inputs hamsterInputs)
    {
        EventManager.Instance.Trigger(EventType.OnStartGame, true);
        _catPlayer.gameObject.SetActive(true);
        _hamsterPlayer.gameObject.SetActive(true);
        foreach (var door in _startDoors)
        {
            if (door.GetPlayerType() == CharacterType.Cat)
            {
                _catPlayer.transform.position = door.GetPlayerPosition().position;
                _catPlayer.ReceiveInputs(catInputs);
            }
            else
            {
                _hamsterPlayer.transform.position = door.GetPlayerPosition().position;
                _hamsterPlayer.ReceiveInputs(hamsterInputs);
            }

            door.Open();
        }
    }

    public IEnumerator DisableByBehaviour(float seconds = 0f)
    {
        yield return new WaitForSeconds(seconds);
        _behaviours ??= mainGame.GetComponentsInChildren<Behaviour>();
        foreach (Behaviour b in _behaviours)
        {
            if (!b ) continue;
            _before[b] = b.enabled;
            if (b.enabled) b.enabled = false;
            if(b.GetComponent<Animator>()) b.GetComponent<Animator>().enabled = true;
            if (b.GetComponent<Rigidbody2D>()) b.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            if(!(b.gameObject.CompareTag("Player") || b.gameObject.CompareTag("Hamster"))) b.gameObject.SetActive(false);
        }
    }

    public void EnableByBehaviour()
    {
        foreach (var keyValue in _before)
        {
            keyValue.Key.enabled = keyValue.Value;

            if (keyValue.Key.GetComponent<Rigidbody2D>())
                keyValue.Key.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            if(!(keyValue.Key.gameObject.CompareTag("Player") || keyValue.Key.gameObject.CompareTag("Hamster"))) keyValue.Key.gameObject.SetActive(true);
        }

        _before.Clear();
    }


    public void SetCatRespawnPoint(Vector3 pos)
    {
        _respawnManager.SetCatRespawnPoint(pos);
    }

    public void SetHamsterRespawnPoint(Vector3 pos)
    {
        _respawnManager.SetHamsterRespawnPoint(pos);
    }

    public Vector3 GetCatRespawnPoint()
    {
        return _respawnManager.GetCatRespawnPoint();
    }

    public Vector3 GetHamsterRespawnPoint()
    {
        return _respawnManager.GetHamsterRespawnPoint();
    }

    public Transform GetCat()
    {
        return _catPlayer.transform;
    }

    public Transform GetHamster()
    {
        return _hamsterPlayer.transform;
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe(EventType.OnLoseGame, OnLoseGame);
        EventManager.Instance.Unsubscribe(EventType.OnFinishGame, OnFinishGame);
        EventManager.Instance.Unsubscribe(EventType.OnResumeGame, OnResumeGame);
        EventManager.Instance.Unsubscribe(EventType.OnPauseGame, OnPauseGame);
    }

    private void OnDestroy()
    {
        EventManager.Instance.Unsubscribe(EventType.OnLoseGame, OnLoseGame);
        EventManager.Instance.Unsubscribe(EventType.OnFinishGame, OnFinishGame);
        EventManager.Instance.Unsubscribe(EventType.OnResumeGame, OnResumeGame);
        EventManager.Instance.Unsubscribe(EventType.OnPauseGame, OnPauseGame);
    }
}