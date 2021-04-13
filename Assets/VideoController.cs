using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour
{
    [SerializeField] RawImage m_rawImage = null;
    [SerializeField] VideoPlayer videoPlayer = null;
    [SerializeField] Slider m_sliderTimeScrub = null;

    public GameObject RightScene;
    public GameObject LeftScene;
    public UnityEngine.UI.Text GestureStatus;

    void Start() {
        m_sliderTimeScrub.maxValue = videoPlayer.frameCount;
        StartCoroutine(PlayVideoCor());
    }

    private void Update() {
        if (videoPlayer.isPlaying == true)
            m_sliderTimeScrub.value = (float)videoPlayer.frame;

        // GameObject인 GestureStatus를 가져와서 해당 Object 안에 들어있는 Text를
        if (GestureStatus.text == "Run")
        {
            videoPlayer.Play();
        } else
        {
            videoPlayer.Pause();
        }

        if (m_sliderTimeScrub.maxValue * 0.98 <  m_sliderTimeScrub.value)
        {
            // 씬 전환 버튼 active
            RightScene.SetActive(true);
            LeftScene.SetActive(true);

            testText.text = "Fin";

            Debug.Log(testText.text);
        }
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