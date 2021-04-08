using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [SerializeField] RawImage m_rawImage = null;
    [SerializeField] VideoPlayer videoPlayer = null;
    [SerializeField] Slider m_sliderTimeScrub = null;

    void Start() {
        m_sliderTimeScrub.maxValue = videoPlayer.frameCount;
        StartCoroutine(PlayVideoCor());
    }

    private void Update() {
        if (videoPlayer.isPlaying == true)
            m_sliderTimeScrub.value = (float)videoPlayer.frame;
    }

    IEnumerator PlayVideoCor() {
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared) {
            yield return null;
        }

        m_rawImage.texture = videoPlayer.texture;
    }

    public void OnClickPlay()
    {
        videoPlayer.Play();
    }

    public void OnClickPause()
    {
        videoPlayer.Pause();
    }

    public void ChageValue() {
        videoPlayer.frame = (long)m_sliderTimeScrub.value;
    }
}