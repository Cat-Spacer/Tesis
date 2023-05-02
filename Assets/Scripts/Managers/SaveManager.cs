using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;

[Serializable]
public class SaveManager : MonoBehaviour
{
    [SerializeField] PlayerSaveData _saveData = new PlayerSaveData();
    [SerializeField] string _saveDataPath;

    void Awake()
    {
        string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/");
        Directory.CreateDirectory(documents + "/" + Application.companyName + "/" + Application.productName + "/" + "/SaveData/");
        _saveDataPath = documents + "/" + Application.companyName + "/" + Application.productName + "/SaveData" + "/PlayerSave.json";
    }

    public void SaveJSON()
    {
        string json = JsonUtility.ToJson(_saveData, true);

        //Debug.Log($"<color=green>Succesfuly saved: {_saveDataPath}</color>");

        File.WriteAllText(_saveDataPath, json);
    }

    public void LoadJSON()
    {
        if (!File.Exists(_saveDataPath)) return;

        string json = File.ReadAllText(_saveDataPath);
        JsonUtility.FromJsonOverwrite(json, _saveData);
        //Debug.Log($"<color=green>Succesfuly data loaded</color>");
    }

    public PlayerSaveData LoadData()
    {
        return _saveData;
    }

    public bool CheckFile()
    {
        //Debug.Log($"<color=yellow>JSON saves exist: {File.Exists(_saveDataPath)}</color>");
        if (!File.Exists(_saveDataPath)) return false;
        return true;
    }
}