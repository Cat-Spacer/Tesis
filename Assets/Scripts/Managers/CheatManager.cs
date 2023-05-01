using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public static CheatManager Instance;
    public Transform player;
    public Transform[] startCheatPosition;
    public Transform[] finishCheatPosition;
    public int currentLvl = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = FindObjectOfType<CustomMovement>().transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StartCheatLevel(currentLvl);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            FinishCheatLevel(currentLvl);
        }
    }

    public void SetCurrentLevel(int lvlIndex)
    {
        currentLvl = lvlIndex;
    }

    public void StartCheatLevel(int lvl)
    {
        player.transform.position = startCheatPosition[lvl].position;
    }

    public void FinishCheatLevel(int lvl)
    {
        player.transform.position = finishCheatPosition[lvl].position;
    }
}