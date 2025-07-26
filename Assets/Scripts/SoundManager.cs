using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFxID
{
    public const int buttonClick = 0;
    public const int countdown = 1;
}

public class SoundManager : MonoBehaviour
{
    static SoundManager instance;
    public static SoundManager Instance => instance;

    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] AudioSource bgmAudioSource;

    [SerializeField] List<AudioClip> sfxList;

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
        //if (sfxlist.count > id)
        //{
        //    sfxaudiosource.playoneshot(sfxlist[id]);
        //}
    }
}
