using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewFatherPanel : MonoBehaviour
{
    //显示界面
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    //隐藏界面
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
