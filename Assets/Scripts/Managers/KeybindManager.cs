using UnityEngine;
using InputKey;

public class KeybindManager : MonoBehaviour
{
    public static KeybindManager Instance;
    public InputDictionary inputDictionary;
    public SaveManager saveManager;
    [SerializeField] private GameObject _popUP;

    private void Awake()
    {
        Instance = this;

        if (saveManager!)
            saveManager = GetComponent<SaveManager>();
        inputDictionary = new InputDictionary();
    }

    void Start()
    {
        saveManager.LoadJSON();

        if (saveManager.LoadData().buttonKeys == null || saveManager.LoadData().buttonValues == null || !saveManager.CheckFile())
            inputDictionary.OnStartIfNotSave();
        else
            inputDictionary.LoadDictionary(saveManager.LoadData().buttonKeys, saveManager.LoadData().buttonValues);
    }

    public void popUPScreen()
    {
        if (_popUP && !_popUP.activeSelf) _popUP.SetActive(true);
    }

    public void Test()
    {
        Debug.Log($"<color=teal>Testing KeybindManager components: saveManager = {saveManager} | inputDictionary = {inputDictionary}</color>");
    }
}