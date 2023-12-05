using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;
//Duty: 處理音樂音效相關的控制
public class AudioController : MonoBehaviour
{
    private bool isPlayingReadyHand = false;

    public AudioClip bgmClip;
    public AudioClip randomSeatClip;
    public AudioClip decideBankerClip;
    public AudioClip openDoorClip;
    public AudioClip groundingFlowerClip;


    public List<AudioClip> chowClips = new List<AudioClip>();
    public List<AudioClip> pongClips = new List<AudioClip>();
    public List<AudioClip> kongClips = new List<AudioClip>();
    public List<AudioClip> readyHandClips = new List<AudioClip>();
    public List<AudioClip> winClips = new List<AudioClip>();
    public List<AudioClip> selfDrawnClips = new List<AudioClip>();
    public List<AudioClip> groundingFlowerClips = new List<AudioClip>();

    public AudioSource bgmSource;
    public AudioSource effectSource;

    public AudioSource vitsSource;

    public AudioMixer audioMixer;

    private void Awake()
    {
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

    public void SetMasterVolume(float volume)    // 控制主音量的函数
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)    // 控制背景音樂音量的函数
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSoundEffectVolume(float volume)    // 控制音效音量的函数
    {
        audioMixer.SetFloat("SoundEffectVolume", volume);
    }


    public void PlayGameBGM()
    {
        isPlayingReadyHand = true;
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayStateAudio(AudioType action)
    {
        switch (action)
        {
            case AudioType.RandomSeat:
                effectSource.clip = randomSeatClip;
                break;
            case AudioType.DecideBanker:
                effectSource.clip = decideBankerClip;
                break;
            case AudioType.OpenDoor:
                effectSource.clip = openDoorClip;
                break;
            case AudioType.GroundingFlower:
                effectSource.clip = groundingFlowerClip;
                break;
        }
        effectSource.loop = false;
        effectSource.Play();
    }

    public void PlayAudioEffect(AudioType action, int characterID)
    {
        int index = characterID - 1;

        switch (action)
        {
            case AudioType.Chow:
                effectSource.clip = chowClips[index];
                break;
            case AudioType.Pong:
                effectSource.clip = pongClips[index];
                break;
            case AudioType.Kong:
                effectSource.clip = kongClips[index];
                break;
            case AudioType.ReadyHand:
                effectSource.clip = readyHandClips[index];
                break;
            case AudioType.Win:
                effectSource.clip = winClips[index];
                break;
            case AudioType.SelfDrawn:
                effectSource.clip = selfDrawnClips[index];
                break;
            case AudioType.GroundingFlower:
                effectSource.clip = groundingFlowerClips[index];
                break;
        }
        effectSource.loop = false;
        effectSource.Play();
    }

    public async Task PlayVitsSpeech(Tuple<string, AudioClip> voiceList)
    {
        if (voiceList != null && voiceList.Item2)
        {
            vitsSource.clip = voiceList.Item2;
        }
        vitsSource.loop = false;
        vitsSource.Play();

        //yield return new WaitForSeconds(voiceList.Item2.length);

        // make delay until speech finish 
        await Task.Delay((int)voiceList.Item2.length * 1000 + 1000);

        // Delete the saved MP3 file after playing
        File.Delete(voiceList.Item1);

        Debug.Log("MP3 downloaded, played, and file deleted.");
    }
}