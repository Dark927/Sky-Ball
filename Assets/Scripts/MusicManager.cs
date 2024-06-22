using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] List<AudioClip> musicList;

    private void Start()
    {
        int musicIndex = Random.Range(0, musicList.Count);
        musicSource.Stop();
        musicSource.clip = musicList[musicIndex];
        musicSource.Play();
    }
}
