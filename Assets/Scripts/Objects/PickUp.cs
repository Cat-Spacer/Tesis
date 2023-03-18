using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] bool _isResource;
    public int _currentLvl;

    private void Start()
    {
        if (_isResource)
        {
            GameManager.Instance.GetAllObjectivesInLevel(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D trig)
    {
        CustomMovement player = trig.GetComponent<CustomMovement>();

        if (player == null) return;
        SoundManager.instance.Play(SoundManager.Types.Item);
        if (_isResource) GameManager.Instance.GetItem();
        transform.parent.gameObject.SetActive(false);
    }    
}
