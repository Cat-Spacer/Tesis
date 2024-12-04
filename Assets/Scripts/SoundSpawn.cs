using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class SoundSpawn : ObjectToSpawn
{
    private GameObject _father = default;
    private AudioSource _source = default;

    public GameObject Father { get { return _father; } }
    public SoundSpawn SetFather(GameObject father)
    {
        _father = father;
        gameObject.transform.parent = _father.transform;
        transform.position = Vector3.zero;
        _source = GetComponent<AudioSource>();
        //if(GameManager.Instance.pause) foreach(AudioSource aS in sources) aS.Pause();
        return this;
    }

    public void PauseMyself()
    {
        if (SoundManager.instance) if (SoundManager.instance.pause) gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (SoundManager.instance) SoundManager.instance.RemoveFromGOList(_source);
    }    
}