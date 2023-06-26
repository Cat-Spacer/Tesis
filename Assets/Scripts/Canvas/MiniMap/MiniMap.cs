using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    [SerializeField] LevelMiniMap[] lvls;
    [SerializeField] Image playerImage;
    [SerializeField] Image objectiveImage;
    Dictionary<int, Image> objectivesDic = new Dictionary<int, Image>();
    int currentLevel = 0;

    private void Start()
    {
        SetPlayerInLevel(0);
    }

    public void SetPlayerInLevel(int lvl)
    {
        currentLevel = lvl;
        playerImage.transform.position = lvls[currentLevel].gameObject.transform.position;
        lvls[currentLevel].SetLevelState(EnumLevelState.Discoverd);
    }

    public void GotItem()
    {
        lvls[currentLevel].SetLevelState(EnumLevelState.ObjCollected);
        foreach (var item in objectivesDic)
        {
            if (item.Key == currentLevel)
            {
                item.Value.gameObject.SetActive(false);
            }
        }
    }

    public void SetObjectiveInMap(int index_arg)
    {
        foreach (var item in objectivesDic)
        {
            if (item.Key == index_arg)
            {
                item.Value.gameObject.SetActive(true);
            }
        }
    }

    // public void CreateObjectiveInMap(List<PickUp> pickUp_arg)
    // {
    //     foreach (var item in pickUp_arg)
    //     {
    //         var objective = Instantiate(objectiveImage, transform);
    //         objective.transform.position = lvls[item._currentLvl].gameObject.transform.position;
    //         objective.gameObject.SetActive(false);
    //         objectivesDic.Add(item._currentLvl, objective);
    //     }
    // }
}