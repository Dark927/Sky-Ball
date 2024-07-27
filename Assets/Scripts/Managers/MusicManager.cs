using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    #region Fields 

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private List<AudioClip> _musicList;

    #endregion


    // -----------------------------------------------------------------------
    // Private Methods
    // -----------------------------------------------------------------------

    #region Private Methods

    private void Start()
    {
        int musicIndex = Random.Range(0, _musicList.Count);
        _musicSource.Stop();
        _musicSource.clip = _musicList[musicIndex];
        _musicSource.Play();
    }

    #endregion
}
