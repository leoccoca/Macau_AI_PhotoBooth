using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFxID
{
    public const int buttonClick = 0;
    public const int countdown = 1;
    public const int langClick = 2;
    public const int cameraShot = 3;
}

public class SoundManager : MonoBehaviour
{
    static SoundManager instance;
    public static SoundManager Instance => instance;

    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] AudioSource bgmAudioSource;

    [SerializeField] List<AudioClip> sfxlist;

    void Awake()
    {
        Debug.Log("### SoundManager Awake");

        // set the singleton instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        //bgmAudioSource.Play();
    }

    public void PlaySfx(int id)
    {
        if (sfxlist.Count > id)
        {
            sfxAudioSource.PlayOneShot(sfxlist[id]);
        }
    }
}
