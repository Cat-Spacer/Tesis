using UnityEngine;
using InputKey;

public class KeybindManager : MonoBehaviour
{
    public static KeybindManager instance = default;
    public InputDictionary inputDictionary = default;
    public JsonSaves JsonSaveManager = new ();
    [SerializeField] private GameObject _popUP = default;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this);

        if (JsonSaveManager == null) JsonSaveManager = new();
        JsonSaveManager.OnAwake();
        inputDictionary = new InputDictionary();
    }

    void Start()
    {
        JsonSaveManager.LoadJson();

        if (JsonSaveManager.LoadData().buttonKeys == null || JsonSaveManager.LoadData().buttonValues == null || !JsonSaveManager.CheckFile())
            inputDictionary.OnStartIfNotSave();
        else
            inputDictionary.LoadDictionary(JsonSaveManager.LoadData().buttonKeys, JsonSaveManager.LoadData().buttonValues);
    }

    public void popUPScreen()
    {
        if (_popUP && !_popUP.activeSelf) _popUP.SetActive(true);
    }

    public void Test()
    {
        Debug.Log($"<color=teal>Testing KeybindManager components: saveManager = {JsonSaveManager} | inputDictionary = {inputDictionary}</color>");
    }
}