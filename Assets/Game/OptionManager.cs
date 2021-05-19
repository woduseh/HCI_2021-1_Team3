using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public UnityEngine.UI.Toggle toggle;

    void Start()
    {
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(Function_Toggle);
    }

    private void Function_Toggle(bool _bool)
    {
        if (_bool == true)
        {
            PlayerPrefs.SetInt("TourGuide", 1);
        }
        else
        {
            PlayerPrefs.SetInt("TourGuide", 0);
        }
    }
}
