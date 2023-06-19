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
        if (_origin && _target) _origin.SetActive(_target.activeInHierarchy);
    }
}
