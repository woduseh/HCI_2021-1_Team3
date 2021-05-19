using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public string HGUSelectScene;
    public string TourSelectScene;
    public string OptionSelectScene;
    public string MainScene;
    public string Scene1;
    public string Scene2;
    public string Scene3;


    public void Quit()
    {
        Application.Quit();
    }

    public void LoadTourSelectScene()
    {
        SceneManager.LoadScene(TourSelectScene);
    }

    public void LoadHGUSelectScene()
    {
        SceneManager.LoadScene(HGUSelectScene);
    }

    public void LoadOptionSelectScene()
    {
        SceneManager.LoadScene(OptionSelectScene);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(MainScene);
    }

    public void LoadScene1()
    {
        SceneManager.LoadScene(Scene1);
    }
    public void LoadScene2()
    {
        SceneManager.LoadScene(Scene2);
    }
    public void LoadScene3()
    {
        SceneManager.LoadScene(Scene3);
    }
}
