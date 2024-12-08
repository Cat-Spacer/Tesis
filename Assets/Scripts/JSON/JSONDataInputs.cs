using UnityEngine;

public class JSONDataInputs : MonoBehaviour
{
    public void SaveData() { KeybindManager.instance.JsonSaveManager.SaveJson(); }

    public void LoadData() { KeybindManager.instance.JsonSaveManager.LoadJson(); }

    public bool CheckData() { return KeybindManager.instance.JsonSaveManager.CheckFile(); }
}
