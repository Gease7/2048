using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPanel : ViewFatherPanel 
{
    public Slider slider_Sound;
    public Slider slider_Music;
    //关闭按钮
    public void OnBtnCloseClick()
    {
        //隐藏当前设置界面
        Hide();

    }

    //音效控制
    public void OnSoundValueChange(float value) 
    {
        //修改音效的大小
        AudioManager.OnSoundVolumeChange(value);
        //保存当前的修改
        PlayerPrefs.SetFloat(Const.Sound, value);
    }

    //音乐控制
    public void OnMusicValueChange(float value) 
    {
        //修改音乐的大小
        AudioManager.OnMusicVolumeChange(value);
        //保存当前的修改
        PlayerPrefs.SetFloat(Const.Music, value);
    }

    public override void Show()
    {
        base.Show();
        //对界面进行初始化
        slider_Sound.value = PlayerPrefs.GetFloat(Const.Sound, 0);//获取上次保存的数值，如果没有就设置为0
        slider_Music.value = PlayerPrefs.GetFloat(Const.Music, 0);
    }
}
