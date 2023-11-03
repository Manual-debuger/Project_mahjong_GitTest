using UnityEngine;
using UnityEngine.Audio;
//Duty: 處理音樂音效相關的控制
public class AudioController : MonoBehaviour
{
    private bool isPlayingReadyHand = false;

    public AudioClip bgmClip;
    public AudioClip chowClip;
    public AudioClip pongClip;
    public AudioClip kongClip;
    public AudioClip groundingFlowerClip;

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

    public void PlayAudioEffect(AudioType action)
    {
        switch(action)
        {
            case AudioType.Chow:
                effectSource.clip = chowClip;
                break;
            case AudioType.Pong:
                effectSource.clip = pongClip;
                break;
            case AudioType.Kong:
                effectSource.clip = kongClip;
                break;
            case AudioType.GroundingFlower:
                effectSource.clip = groundingFlowerClip;
                break;
        }
        effectSource.loop = false;
        effectSource.Play();
    }

    public void PlayVitsSpeech(AudioClip speechChip)
    {
        while (speechChip != null)
        {
            vitsSource.clip = speechChip;
        }
        vitsSource.loop = false;
        vitsSource.Play();
    }
}
