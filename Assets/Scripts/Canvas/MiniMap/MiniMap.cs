using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MiniMap : MonoBehaviour
{
    [SerializeField] LevelMiniMap[] lvls;
    [SerializeField] Image playerImage;
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
        lvls[currentLevel].SetLevelState(EnumLevelState.ObjFound);
    }
}
