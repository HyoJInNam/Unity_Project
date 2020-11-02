using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionManager : MonoBehaviour
{
    public GameObject pause;
    public GameObject canvas;

    public void Pause(GameObject canvas)
    {
        canvas.SetActive((canvas.activeSelf) ? false : true);
        pause.SetActive((canvas.activeSelf) ? false : true);
    }

    public void Back(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }

    public void Retry(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Continue(GameObject pause)
    {
        pause.SetActive((pause.activeSelf) ? false : true);
        canvas.SetActive((pause.activeSelf) ? false : true);
    }
    
}
