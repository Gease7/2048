using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public Number number;       //当前这个格子的数字

    //判断格子中是否有数字
    public bool isHaveNumber()
    {
        return number != null;
    }

    //获取数字
    public Number GetNumber()
    {
        return number;
    }

    //设置数字
    public void SetNumber(Number number)
    {
        this.number = number;
    }

}
