using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePanel : MonoBehaviour
{
    #region UI控件变量
    public SetPanel setPanel;               //设置菜单
    public WinPanel winPanel;               //胜利界面
    public GameOverPanel gameOverPanel;     //结束界面 

    public Text text_CurrentScore;          //当前分数
    public Text text_BestScore;             //最高分数
    public Button btn_BackMenu;             //返回菜单按钮
    public Button btn_Restart;              //重新开始按钮
    public Button btn_Set;                  //设置按钮
    public Button btn_LastStep;             //上一步按钮

    public Transform gridParent;            //格子的父物体
    #endregion

    #region  属性和变量
    public Dictionary<int, Vector2> grid_config = new Dictionary<int, Vector2>()
    { { 4, new Vector2(150,13.2f)},  { 5, new Vector2(120, 10.56f) }, {6, new Vector2(100, 9.24f) } };      //存储不同难度下的格子数量

    private int row; //行
    private int col; //列

    public GameObject gridPrefab;
    public GameObject numberPrefab;

    public Slot[][] grids = null;           //创建一个二维数组来保存格子
    public List<Slot> canCreateNumberGrids = new List<Slot>();          //可以创建数字的格子

    public Vector3 pointerDownPos, pointerUpPos;

    private bool isNeedNumber;
    public int currentScore;

    public StepModel lastStepModel;

    #endregion

    #region 脚本周期
    private void Awake()
    {
        //初始化界面更新
        InitPanelMessage();
        //初始化格子
        InitGrid();
        //创建第一个数字
        CreatNumber();
    }
    #endregion

    #region  游戏逻辑
    //初始化格子
    public void InitGrid()
    {
        //获取格子的数量 
        int gridNum = PlayerPrefs.GetInt(Const.GameMode, 4);
        GridLayoutGroup gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraintCount = gridNum;
        //获得字典中存储的格子的大小并赋值
        float size = grid_config[gridNum].x;
        gridLayoutGroup.cellSize = new Vector2(size, size);
        //获得字典 中存储格子的间距并赋值
        float spacing = grid_config[gridNum].y;
        gridLayoutGroup.spacing = new Vector2(spacing, spacing);

        //初始化格子
        grids = new Slot[gridNum][];

        //创建具体数量的格子
        row = gridNum;
        col = gridNum;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (grids[i] == null)
                {
                    grids[i] = new Slot[gridNum];
                }
                //创建i行j列的格子
                grids[i][j] = CreatGrid();
            }
        }
    }

    //创建格子
    public Slot CreatGrid()
    {
        //实例化格子的预制体
        GameObject gameObject = Instantiate(gridPrefab, gridParent);
        return gameObject.GetComponent<Slot>();
    }

    //创建数字
    public void CreatNumber()
    {
        //找到这个数字所在的格子
        canCreateNumberGrids.Clear();
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                //判断这个格子是否有数字
                if (!grids[i][j].isHaveNumber())
                {
                    //添加空格子
                    canCreateNumberGrids.Add(grids[i][j]);
                }
            }
        }
        //如果空格子数量为0，那么就不能创建数字
        if (canCreateNumberGrids.Count == 0)
            return;
        //随机一个格子
        int index = Random.Range(0, canCreateNumberGrids.Count);
        //创建数字并把数字放到格子里
        GameObject gameObject = Instantiate(numberPrefab, canCreateNumberGrids[index].transform);
        //对数字进行初始化
        gameObject.GetComponent<Number>().Init(canCreateNumberGrids[index]);

    }

    public void CreatNumber(Slot slot, int number)
    {
        GameObject gameObject = Instantiate(numberPrefab, slot.transform);
        gameObject.GetComponent<Number>().Init(slot);
        gameObject.GetComponent<Number>().SetNumber(number);
    }

    public MoveType CaculateMoveType()
    {
        if (Mathf.Abs(pointerUpPos.x - pointerDownPos.x) > Mathf.Abs(pointerUpPos.y - pointerDownPos.y))
        {
            //左右移动
            if (pointerUpPos.x - pointerDownPos.x > 0)
            {
                //向右移动
                return MoveType.RIGHT;
            }
            else
            {
                //向左移动
                return MoveType.LEFT;
            }
        }
        else
        {
            //上下移动
            if (pointerUpPos.y - pointerDownPos.y > 0)
            {
                //向右移动
                return MoveType.TOP;
            }
            else
            {
                //向左移动
                return MoveType.DOWN;
            }
        }
    }

    public void MoveNumber(MoveType moveType)
    {
        switch (moveType)
        {
            case MoveType.TOP:
                for (int j = 0; j < col; j++)
                {
                    for (int i = 1; i < row; i++)
                    {
                        if (grids[i][j].isHaveNumber())
                        {
                            Number number = grids[i][j].GetNumber();

                            for (int m = i - 1; m >= 0; m--)
                            {
                                Number targetNumber = null;
                                if (grids[m][j].isHaveNumber())
                                {
                                    targetNumber = grids[m][j].GetNumber();
                                }
                                HandleNumber(number, targetNumber, grids[m][j]);
                                if (targetNumber != null)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
            case MoveType.DOWN:
                for (int j = 0; j < col; j++)
                {
                    for (int i = row - 2; i >= 0; i--)
                    {
                        if (grids[i][j].isHaveNumber())
                        {
                            Number number = grids[i][j].GetNumber();
                            for (int m = i + 1; m < row; m++)
                            {
                                Number targetNumber = null;
                                if (grids[m][j].isHaveNumber())
                                {
                                    targetNumber = grids[m][j].GetNumber();
                                }
                                HandleNumber(number, targetNumber, grids[m][j]);
                                if (targetNumber != null)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
            case MoveType.LEFT:
                for (int i = 0; i < row; i++)
                {
                    for (int j = 1; j < col; j++)
                    {
                        if (grids[i][j].isHaveNumber())
                        {
                            Number number = grids[i][j].GetNumber();
                            for (int m = j - 1; m >= 0; m--)
                            {
                                Number targetNumber = null;
                                if (grids[i][m].isHaveNumber())
                                {
                                    targetNumber = grids[i][m].GetNumber();
                                }
                                HandleNumber(number, targetNumber, grids[i][m]);
                                if (targetNumber != null)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
            case MoveType.RIGHT:
                for (int i = 0; i < row; i++)
                {
                    for (int j = col - 2; j >= 0; j--)
                    {
                        if (grids[i][j].isHaveNumber())
                        {
                            Number number = grids[i][j].GetNumber();
                            for (int m = j + 1; m < col; m++)
                            {
                                Number targetNumber = null;
                                if (grids[i][m].isHaveNumber())
                                {
                                    targetNumber = grids[i][m].GetNumber();
                                }
                                HandleNumber(number, targetNumber, grids[i][m]);
                                if (targetNumber != null)
                                {
                                    break;
                                }
                                //if (grids[i][m].isHaveNumber())
                                //{
                                //    //判断能不能合并
                                //    if (number.GetNumber() == grids[i][m].GetNumber().GetNumber())
                                //    {
                                //        //合并
                                //        grids[i][m].GetNumber().Merge();
                                //        //销毁当前的这个数字
                                //        number.GetSlot().SetNumber(null);
                                //        GameObject.Destroy(number.gameObject);
                                //    }
                                //    break;
                                //}
                                //else
                                //{
                                //    //没数字 移动上去
                                //    number.MoveToSlot(grids[i][m]);
                                //}
                            }
                        }
                    }
                }
                break;
            default:
                break;
        }
    }

    //处理数字
    public void HandleNumber(Number current, Number target, Slot targetSlot)
    {
        if (target != null)
        {
            //判断能不能合并
            if (current.IsMerge(target))
            {

                isNeedNumber = true;
                //销毁当前的数字 
                current.GetSlot().SetNumber(null);
                //GameObject.Destroy(current.gameObject);
                current.DestroyOnMoveEnd(target.GetSlot());
                target.Merge();
            }
        }
        else
        {
            current.MoveToSlot(targetSlot);
            isNeedNumber = true;
        }
    }

    //恢复数字
    public void ResetNumberStatus()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (grids[i][j].isHaveNumber())
                {
                    grids[i][j].GetNumber().status = NumberStatus.Normal;
                }
            }
        }
    }

    //判断游戏是否失败
    public bool IsGameOver()
    {

        //判断格子是否满了
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (!grids[i][j].isHaveNumber())
                {
                    return false;
                }
            }
        }
        //判断有没有数字能合并
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Slot up = IsHaveGrid(i - 1, j) ? grids[i - 1][j] : null;
                Slot down = IsHaveGrid(i + 1, j) ? grids[i + 1][j] : null;
                Slot left = IsHaveGrid(i, j - 1) ? grids[i][j - 1] : null;
                Slot right = IsHaveGrid(i, j + 1) ? grids[i][j + 1] : null;

                if (up != null)
                {
                    if (grids[i][j].GetNumber().IsMerge(up.GetNumber()))
                        return false;
                }
                if (down != null)
                {
                    if (grids[i][j].GetNumber().IsMerge(down.GetNumber()))
                        return false;
                }
                if (left != null)
                {
                    if (grids[i][j].GetNumber().IsMerge(left.GetNumber()))
                        return false;
                }
                if (right != null)
                {
                    if (grids[i][j].GetNumber().IsMerge(right.GetNumber()))
                        return false;
                }
            }
        }
        return true;
    }

    public bool IsHaveGrid(int i, int j)
    {
        if (i >= 0 && i < row && j >= 0 && j < col)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region  事件监听
    public void OnPointerDown()
    {
        pointerDownPos = Input.mousePosition;
    }

    public void OnPointerUp()
    {
        pointerUpPos = Input.mousePosition;
        if (Vector3.Distance(pointerUpPos, pointerDownPos) < 100)
        {
            return;
        }
        //保存数据
        lastStepModel.UpdateData(this.currentScore, PlayerPrefs.GetInt(Const.BestScore, 0), grids);
        btn_LastStep.interactable = true;

        MoveType moveType = CaculateMoveType();
        MoveNumber(moveType);
        //产生一个新的数字
        if (isNeedNumber)
        {
            CreatNumber();
            isNeedNumber = false;
        }

        //把所有数字恢复正常状态
        ResetNumberStatus();

        //判断游戏是否结束？
        if (IsGameOver())
        {
            GameOver();
        }



    }

    //返回菜单 
    public void OnBackMenuClick()
    {
        SceneManager.LoadSceneAsync(0);
    }

    //设置界面
    public void OnSetClick()
    {
        setPanel.Show();
    }

    public void OnLastClick()
    {
        BackToLastStep();
        btn_LastStep.interactable = false;
    }
    #endregion

    #region 界面更新

    public void InitPanelMessage()
    {
        this.text_BestScore.text = PlayerPrefs.GetInt(Const.BestScore, 0).ToString();
        lastStepModel = new StepModel();
        btn_LastStep.interactable = false;
    }
    public void AddScore(int score)
    {
        currentScore += score;
        UpdateScore(currentScore);

        //判断当前的分数是不是最高的分数
        if (currentScore > PlayerPrefs.GetInt(Const.BestScore, 0))
        {
            PlayerPrefs.SetInt(Const.BestScore, currentScore);
            UpdateBestScore(currentScore);
        }
    }

    public void UpdateScore(int score)
    {
        this.text_CurrentScore.text = score.ToString();
    }
    public void RestartScore()
    {
        currentScore = 0;
        UpdateScore(currentScore);
    }
    public void UpdateBestScore(int bestscore)
    {
        this.text_BestScore.text = bestscore.ToString();
    }
    #endregion

    #region 游戏流程
    public void BackToLastStep()
    {
        //分数
        currentScore = lastStepModel.score;
        UpdateScore(lastStepModel.score);

        PlayerPrefs.GetInt(Const.BestScore, lastStepModel.bestScore);
        UpdateBestScore(lastStepModel.bestScore);

        //数字 
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (lastStepModel.numbers[i][j] == 0)
                {
                    if (grids[i][j].isHaveNumber()) {
                        GameObject.Destroy(grids[i][j].GetNumber().gameObject);
                        grids[i][j].SetNumber(null);
                    }
                    
                }
                else if(lastStepModel.numbers[i][j]!=0)
                {
                    if (grids[i][j].isHaveNumber())
                    {
                        //修改数字
                        grids[i][j].GetNumber().SetNumber(lastStepModel.numbers[i][j]);
                    }
                    else
                    {
                        //创建数字
                        CreatNumber(grids[i][j], lastStepModel.numbers[i][j]);
                        
                    }
                }
            }
        }

    }

    //重新开始 
    public void RestartGame()
    {
        //数据清空
        btn_LastStep.interactable = false;
        //清空分数
        RestartScore();
        //清空数字
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (grids[i][j].isHaveNumber())
                {
                    GameObject.Destroy(grids[i][j].GetNumber().gameObject);
                    grids[i][j].SetNumber(null);
                }
            }
        }

        //创建一个数字
        CreatNumber();
    }

    //游戏胜利
    public void GameWin()
    {
        winPanel.Show();
    }
    //游戏结束
    public void GameOver()
    {
        gameOverPanel.Show();
    }

    #endregion

}

