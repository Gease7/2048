using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;

    public AudioSource bgm, sound;
    public AudioClip bgm_Clip, sound_Clip;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
        PlayBGM();

        //获取到保存的声音大小
        _instance.bgm.volume = PlayerPrefs.GetFloat(Const.Music, 0.5f);
        _instance.sound.volume = PlayerPrefs.GetFloat(Const.Sound, 0.5f);
    }
    public static void PlayBGM()
    {
        _instance.bgm.clip = _instance.bgm_Clip;
        _instance.bgm.loop = true;
        _instance.bgm.Play();

    }
    public static void PlaySound()
    {
        _instance.sound.PlayOneShot(_instance.sound_Clip);
    }

    public static  void OnMusicVolumeChange(float value)
    {
        _instance.bgm.volume = value;
    }
    public static void OnSoundVolumeChange(float value)
    {
        _instance.sound.volume = value;
    }
}
