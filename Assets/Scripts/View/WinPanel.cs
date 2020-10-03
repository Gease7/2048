using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : ViewFatherPanel
{
    public void OnRestartClick()
    {
        GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>().RestartGame();
        this.Hide();
    }
    public void OnBackMenuClik()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
