using UnityEngine;

public class ActiveIfActive : MonoBehaviour
{
    [SerializeField] private GameObject _origin, _target;

    void Update()
    {
        ForTheAlliance();
    }

    private void ForTheAlliance()
    {
        _origin.SetActive(_target.activeInHierarchy);
    }
}
