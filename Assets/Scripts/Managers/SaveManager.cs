using UnityEngine;
using System.IO;
using System;

[Serializable]
public class SaveManager : MonoBehaviour
{
    [SerializeField] private PlayerSaveData _saveData = new PlayerSaveData();
    [SerializeField] private string _saveDataPath = default;

    void Awake()
    {
        string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/");
        Directory.CreateDirectory(documents + "/" + Application.companyName + "/" + Application.productName + "/" + "/SaveData/");
        _saveDataPath = documents + "/" + Application.companyName + "/" + Application.productName + "/SaveData" + "/PlayerSave.json";
    }

    public void SaveJSON()
    {
        string json = JsonUtility.ToJson(_saveData, true);

        File.WriteAllText(_saveDataPath, json);
    }

    public void LoadJSON()
    {
        if (!File.Exists(_saveDataPath)) return;

        string json = File.ReadAllText(_saveDataPath);
        JsonUtility.FromJsonOverwrite(json, _saveData);
    }

    public PlayerSaveData LoadData()
    {
        return _saveData;
    }

    public bool CheckFile()
    {
        if (!File.Exists(_saveDataPath)) return false;
        return true;
    }
}