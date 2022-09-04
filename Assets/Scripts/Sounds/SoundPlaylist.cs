using System.Collections.Generic;
using UnityEngine;

public class SoundPlaylist : MonoBehaviour
{
    [SerializeField] private List<AudioClip> playList = new List<AudioClip>();
    [SerializeField] private AudioSource audioSource;
    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        if (FindObjectOfType<SoundManager>() == null) return;
        SoundManager soundManager = FindObjectOfType<SoundManager>();
        for (int i = 0; i < soundManager.sounds.Length; i++)
        {
            if (soundManager.sounds[i].audioMixerGroup != null && soundManager.sounds[i].audioMixerGroup.name == "Music")
            {
                playList.Add(soundManager.sounds[i].clip);
            }
        }
    }

    void Update()
    {
        if (!audioSource.isPlaying && audioSource != null)
        {
            audioSource.clip = GetRandomClip();
            audioSource.Play();
        }   
    }

    private AudioClip GetRandomClip()
    {
        return playList[Random.Range(0, playList.Count)];
    }
}
