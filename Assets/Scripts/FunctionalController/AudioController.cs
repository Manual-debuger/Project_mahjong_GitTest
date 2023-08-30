using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Duty: 處理音樂音效相關的控制
public class AudioController : MonoBehaviour
{
    private bool isPlayingReadyHand = false;

    public AudioClip bgmClip;
    public AudioClip chowClip;
    public AudioClip pongClip;
    public AudioClip kongClip;
    public AudioClip groundingFlowerClip;

    private AudioSource bgmSource;
    private AudioSource audioSource;

    private void Awake()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayGameBGM();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGameBGM()
    {
        isPlayingReadyHand = true;
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.volume = 0.3f;
        bgmSource.Play();
    }

    public void PlayAudioEffect(AudioType action)
    {
        switch(action)
        {
            case AudioType.Chow:
                audioSource.clip = chowClip;
                break;
            case AudioType.Pong:
                audioSource.clip = pongClip;
                break;
            case AudioType.Kong:
                audioSource.clip = kongClip;
                break;
            case AudioType.GroundingFlower:
                audioSource.clip = groundingFlowerClip;
                break;
        }
        audioSource.loop = false;
        audioSource.Play();
    }
}
