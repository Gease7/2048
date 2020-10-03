using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public SelectPanel selectPanel;
    public SetPanel setPanel;

    //开始游戏
    public void OnStartGameClick()
    {
        //显示选择模式的界面
        selectPanel.Show();
    }
    
    //设置菜单
    public void OnSetClick() 
    {
        //显示设置界面
        setPanel.Show();
    }


    //退出游戏
    public void OnExitClick() 
    {
        //退出游戏
        Application.Quit();
    }
}
