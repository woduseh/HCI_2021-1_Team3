using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public string SampleScene_Right;
    public string SampleScene_Left;
    public void SceneChangeRight()
    {
        SceneManager.LoadScene(SampleScene_Right);
    }

    public void SceneChangeLeft()
    {
        SceneManager.LoadScene(SampleScene_Left);
    }
}
