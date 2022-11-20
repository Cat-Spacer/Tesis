using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMiniMap : MonoBehaviour
{

    [SerializeField] int lvlIndex;
    [SerializeField] bool rightDoor, leftDoor, topDoor, bottomDoor;
    [SerializeField] GameObject rightDoorObj, leftDoorObj, topDoorObj, bottomDoorObj;
    [SerializeField] Color undiscoverdLevel;
    [SerializeField] Color discoverdLevel;
    [SerializeField] Color objFoundLevel;
    [SerializeField] Image myColor;
    private void Awake()
    {
        myColor = GetComponent<Image>();
        if (lvlIndex == 0) myColor.color = discoverdLevel;
        else myColor.color = undiscoverdLevel;
    }
    void Start()
    {
        if (rightDoor) rightDoorObj.SetActive(true);
        if (leftDoor) leftDoorObj.SetActive(true); ;
        if (topDoor) topDoorObj.SetActive(true); ;
        if (bottomDoor) bottomDoorObj.SetActive(true);
    }
    public void SetLevelState(EnumLevelState state)
    {
        switch (state)
        {
            case EnumLevelState.Undiscoverd:
                myColor.color = undiscoverdLevel;
                break;
            case EnumLevelState.Discoverd:
                myColor.color = discoverdLevel;
                break;
            case EnumLevelState.ObjDiscoverd:
                myColor.color = discoverdLevel;
                break;
            case EnumLevelState.ObjCollected:
                myColor.color = objFoundLevel;
                break;
            default:
                break;
        }
    }
}
