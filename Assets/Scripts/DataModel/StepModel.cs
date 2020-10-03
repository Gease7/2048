using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepModel
{
    public int score;
    public int bestScore;

    public int[][] numbers;

    public void  UpdateData(int score, int bestScore, Slot[][] grids)
    {
        this.score = score;
        this.bestScore = bestScore;
        //如果为空就初始化
        if(numbers == null)
            numbers = new int[grids.Length][];

        for (int i = 0; i < grids.Length; i++)
        {
            for (int j = 0; j <grids[i].Length; j++)
            {
                //如果为空就初始化
                if (numbers[i]==null)
                {
                    numbers[i] = new int[grids[i].Length];
                }
                numbers[i][j] = grids[i][j].isHaveNumber()?grids[i][j].GetNumber().GetNumber():0;
            }
        }
    }
}
