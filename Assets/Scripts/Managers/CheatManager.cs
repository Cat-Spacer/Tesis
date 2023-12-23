using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public static CheatManager Instance = default;
    public Transform player = default;
    public Transform[] startCheatPosition = default, finishCheatPosition = default;
    public int currentLvl = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(!player) player = FindObjectOfType<CustomMovement>().transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)) StartCheatLevel(currentLvl);

        if (Input.GetKeyDown(KeyCode.F2)) FinishCheatLevel(currentLvl);
    }

    public void SetCurrentLevel(int lvlIndex) { currentLvl = lvlIndex; }

    public void StartCheatLevel(int lvl) { player.transform.position = startCheatPosition[lvl].position; }

    public void FinishCheatLevel(int lvl) { player.transform.position = finishCheatPosition[lvl].position; }
}