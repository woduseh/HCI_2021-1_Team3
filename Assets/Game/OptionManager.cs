using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public UnityEngine.UI.Toggle TourGuide;
    public UnityEngine.UI.Toggle Cal_Indicator;
    public UnityEngine.UI.Toggle Minimap;

    void Start()
    {
        TourGuide.onValueChanged.RemoveAllListeners();
        TourGuide.onValueChanged.AddListener(TourGuideToggle);
        Cal_Indicator.onValueChanged.RemoveAllListeners();
        Cal_Indicator.onValueChanged.AddListener(Cal_IndicatorToggle);
        Minimap.onValueChanged.RemoveAllListeners();
        Minimap.onValueChanged.AddListener(MinimapToggle);
    }

    private void TourGuideToggle(bool _bool)
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

    private void Cal_IndicatorToggle(bool _bool)
    {
        if (_bool == true)
        {
            PlayerPrefs.SetInt("Cal_Indicator", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Cal_Indicator", 0);
        }
    }

    private void MinimapToggle(bool _bool)
    {
        if (_bool == true)
        {
            PlayerPrefs.SetInt("Minimap", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Minimap", 0);
        }
    }
}
