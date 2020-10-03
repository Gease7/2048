using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectPanel : ViewFatherPanel
{
    //点击选择难度按钮
    public void OnSelectModeClick(int count) 
    {
        //选择模式
        PlayerPrefs.SetInt(Const.GameMode, count);
        //跳转场景 到 游戏场景
        AsyncOperation asyncOperation =  SceneManager.LoadSceneAsync(1);//异步加载,
        
        //asyncOperation.progress//用来显示进度条

    }

}
