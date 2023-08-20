using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

//Duty: 處理右上角的設定按鈕
public class SettingUIButton : MonoBehaviour
{
    [SerializeField] private GameObject Setting;
    [SerializeField] private Button CloseButton;
    [SerializeField] private Slider SoundSlider;
    [SerializeField] private Slider MusicSlider;
    public event EventHandler<FloatEventArgs> SetMusicEvent;
    public event EventHandler<FloatEventArgs> SetSoundEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenSetting()
    {
        Setting.SetActive(true);
    }

    public void CloseSetting()
    {
        //Debug.Log(index);
        Setting.SetActive(false);
    }

    public void SetMusic()
    {
        //Debug.Log("SetMusic");
        SetMusicEvent?.Invoke(this, new FloatEventArgs(MusicSlider.value));
    }

    public void SetSound()
    {
        //Debug.Log("SetSound");
        SetSoundEvent?.Invoke(this, new FloatEventArgs(SoundSlider.value));
    }
}
