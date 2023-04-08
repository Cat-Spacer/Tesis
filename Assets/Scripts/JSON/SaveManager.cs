using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Xml.Linq;

[Serializable]
public class SaveManager : MonoBehaviour
{
    [SerializeField] PlayerSaveData saveData;
    [SerializeField] string _saveDataPath;

    void Awake()
    {
        string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/");
        Debug.Log(documents);
        Directory.CreateDirectory(documents + "/CatroInSpace/" + "/SaveData/");

        _saveDataPath = documents + "/CatroInSpace" + "/SaveData" + "/PlayerSave.json";
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.S))
            SaveJSON();
        else if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.L))
            LoadJSON();
    }

    public void SaveJSON()
    {
        string json = JsonUtility.ToJson(saveData, true);

        Debug.Log($"<color=green>Succesfuly saved: {_saveDataPath}</color>");

        File.WriteAllText(_saveDataPath, json);
    }

    public void LoadJSON()
    {
        if (!File.Exists(_saveDataPath)) return;

        string json = File.ReadAllText(_saveDataPath);
        JsonUtility.FromJsonOverwrite(json, saveData);
        Debug.Log($"<color=green>Succesfuly saved data loaded</color>");
    }

    public bool CheckFile()
    {
        Debug.Log($"<color=yellow>JSON saves exist: {File.Exists(_saveDataPath)}</color>");
        if (!File.Exists(_saveDataPath)) return false;
        return true;
    }
}