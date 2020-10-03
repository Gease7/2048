using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Number : MonoBehaviour
{
    private Image bg;
    private Text number_text;
    private Slot inSlot;

    public NumberStatus status;

    private float spawnScaleTime = 1;
    private bool isPlaySpawnAnim;

    private float mergeScaleTime = 1;
    private float mergeScaleTimeBack = 1;
    private bool isPlayMergeAnim;

    private float movePosTime = 1;
    private bool isMoving;
    private bool isDestroyOnMoveEnd;
    private Vector3 startMovPos;

    public Color[] bg_Colors;
    public List<int> number_index;

    public int index;
    private void Awake()
    {
        //初始化
        bg = transform.GetComponent<Image>();
        number_text = gameObject.GetComponentInChildren<Text>();
        index = Random.Range(0, 2);
    }

    //初始化
    public void Init(Slot slot)
    {
        slot.SetNumber(this);
        //设置所在的格子
        this.SetSlot(slot);
        //设置初始化的数字
        if (index == 1)
            this.SetNumber(2);
        else
            this.SetNumber(4);
        status = NumberStatus.Normal;

        PlaySpawnAnimation();
    }

    //设置格子
    public void SetSlot(Slot slot)
    {
        this.inSlot = slot;
    }
    //获取格子
    public Slot GetSlot()
    {
        return this.inSlot;
    }

    //设置数字
    public void SetNumber(int number)
    {
        this.number_text.text = number.ToString();
        this.bg.color = this.bg_Colors[number_index.IndexOf(number)];
    }

    //获取数字
    public int GetNumber()
    {
        return int.Parse(number_text.text);
    }

    //把数字移动到某个格子下面
    public void MoveToSlot(Slot slot)
    {
        transform.SetParent(slot.transform);
        //transform.localPosition = Vector3.zero;
        startMovPos = transform.localPosition;

        PlayMoveAnimation();

        this.GetSlot().SetNumber(null);
        //设置格子
        slot.SetNumber(this);
        this.SetSlot(slot);
    }

    public void DestroyOnMoveEnd(Slot slot)
    {
        transform.SetParent(slot.transform);
        startMovPos = transform.localPosition;

        PlayMoveAnimation();
        isDestroyOnMoveEnd = true;
    }

    //合并
    public void Merge()
    {
        GamePanel gamePanel = GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>();
        gamePanel.AddScore(this.GetNumber());

        int resultNumber = this.GetNumber() * 2;
        this.SetNumber(this.GetNumber() * 2);
        if (resultNumber == 2048)
        {
            gamePanel.GameWin();
        }
        status = NumberStatus.Notmerge;
        PlayMergeAnimation();
        AudioManager.PlaySound();
    }


    //判断能不能合并
    public bool IsMerge(Number number)
    {
        if (this.GetNumber() == number.GetNumber() && number.status == NumberStatus.Normal)
            return true;
        return false;
    }


    public void PlaySpawnAnimation()
    {
        spawnScaleTime = 0;
        isPlaySpawnAnim = true;
    }

    public void PlayMergeAnimation()
    {
        mergeScaleTime = 0;
        mergeScaleTimeBack = 0;
        isPlayMergeAnim = true;
    }

    public void PlayMoveAnimation()
    {
        movePosTime = 0;
        isMoving = true;
    }

    public void Update()
    {
        //创建的动画
        if (isPlaySpawnAnim)
        {
            if (spawnScaleTime <= 1)
            {
                spawnScaleTime += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, spawnScaleTime);
            }
            else
                isPlaySpawnAnim = false;
        }

        //合并的动画 
        if (isPlayMergeAnim)
        {
            if (mergeScaleTime <= 1 && mergeScaleTimeBack == 0)
            {
                mergeScaleTime += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, mergeScaleTime);
            }
            if (mergeScaleTime >= 1 && mergeScaleTimeBack <= 1)
            {
                mergeScaleTimeBack += Time.deltaTime * 4;
                transform.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one, mergeScaleTime);
            }
            if (mergeScaleTime >= 1 && mergeScaleTimeBack >= 1)
                isPlayMergeAnim = false;
        }


        //移动的动画
        if (isMoving)
        {

            //this.GetSlot().transform.SetSiblingIndex(0);
            //this.transform.SetSiblingIndex(1);
            movePosTime += Time.deltaTime * 5;
            transform.localPosition = Vector3.Lerp(startMovPos, Vector3.zero, movePosTime);
            if (movePosTime >= 1)
            {
                isMoving = false;
                if (isDestroyOnMoveEnd)
                    GameObject.Destroy(this.gameObject);
            }

        }
    }

}
