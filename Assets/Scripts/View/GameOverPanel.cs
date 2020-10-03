using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverPanel : ViewFatherPanel
{
    public void OnRestartClick()
    {
        //调用GamePanel中的重新开始
        GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>().RestartGame();
        this.Hide();
    }
    public void OnBackMenuClik()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
