using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    public void PlayGame (int index)
    {
        SceneManager.LoadScene(index);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
