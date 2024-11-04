using UnityEngine;

public class JSONDataInputs : MonoBehaviour
{
    public void SaveData() { KeybindManager.instance.saveManager.SaveJSON(); }

    public void LoadData() { KeybindManager.instance.saveManager.LoadJSON(); }

    public bool CheckData() { return KeybindManager.instance.saveManager.CheckFile(); }
}
