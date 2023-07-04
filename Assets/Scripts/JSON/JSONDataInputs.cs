using UnityEngine;

public class JSONDataInputs : MonoBehaviour
{
    public void SaveData() { KeybindManager.Instance.saveManager.SaveJSON(); }

    public void LoadData() { KeybindManager.Instance.saveManager.LoadJSON(); }

    public bool CheckData() { return KeybindManager.Instance.saveManager.CheckFile(); }
}
