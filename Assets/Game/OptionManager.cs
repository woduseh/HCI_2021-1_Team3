using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public UnityEngine.UI.Toggle TourGuide;
    public UnityEngine.UI.Toggle Cal_Indicator;

    void Start()
    {
        TourGuide.onValueChanged.RemoveAllListeners();
        TourGuide.onValueChanged.AddListener(TourGuideToggle);
        Cal_Indicator.onValueChanged.RemoveAllListeners();
        Cal_Indicator.onValueChanged.AddListener(Cal_IndicatorToggle);
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
}
