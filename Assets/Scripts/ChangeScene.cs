using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public void SceneChange(string nextSceneName)
    {
        SceneManager.LoadScene(nextSceneName);
        TimeScaleCheck();
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("01_MazeGameScene"); // 게임 씬으로 전환
        TimeScaleCheck();
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("00_LoadScene"); // 메인 메뉴 씬으로 전환
        TimeScaleCheck();
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void MySceneLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        TimeScaleCheck();
    }

    private void TimeScaleCheck()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1f;
        }
    }
}
