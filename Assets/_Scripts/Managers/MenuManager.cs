using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
  public void EnterGame()
    {
        SceneManager.LoadScene("BoScene");
    }

    public void QuitGame()
    {
        Debug.Log("Exiting game...");

        // 检查是否在Unity编辑器中运行
#if UNITY_EDITOR
        // 如果是在编辑器中，停止播放模式
        EditorApplication.isPlaying = false;
#else
            // 在构建的游戏中，退出游戏
            Application.Quit();
#endif
    }
}
