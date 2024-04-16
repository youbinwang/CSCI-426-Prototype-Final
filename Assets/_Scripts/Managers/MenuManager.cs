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

        // ����Ƿ���Unity�༭��������
#if UNITY_EDITOR
        // ������ڱ༭���У�ֹͣ����ģʽ
        EditorApplication.isPlaying = false;
#else
            // �ڹ�������Ϸ�У��˳���Ϸ
            Application.Quit();
#endif
    }
}
