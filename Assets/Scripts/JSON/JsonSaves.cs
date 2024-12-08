using UnityEngine;
using System.IO;
using System;

[Serializable]
public class JsonSaves
{
    [SerializeField] private PlayerSaveData _saveData = new PlayerSaveData();
    [SerializeField] private string _saveDataPath = default;

    public void OnAwake()
    {
        string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/");
        Directory.CreateDirectory(documents + "/" + Application.companyName + "/" + Application.productName + "/" + "/SaveData/");
        _saveDataPath = documents + "/" + Application.companyName + "/" + Application.productName + "/SaveData" + "/PlayerSave.json";
        if(!CheckFile()) SaveJson();
        LoadJson();
    }

    public void SaveJson()
    {
        string json = JsonUtility.ToJson(_saveData, true);

        File.WriteAllText(_saveDataPath, json);
    }

    public void LoadJson()
    {
        if (!CheckFile()) return;

        string json = File.ReadAllText(_saveDataPath);
        JsonUtility.FromJsonOverwrite(json, _saveData);
    }

    public PlayerSaveData LoadData()
    {
        return _saveData;
    }

    public bool CheckFile()
    {
        return File.Exists(_saveDataPath);
    }
}
