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

    public string RightScene;
    public string LeftScene;
    public GameObject RightArrow;
    public GameObject LeftArrow;
    public UnityEngine.UI.Text GestureStatus;

    void Start() {
        m_sliderTimeScrub.maxValue = videoPlayer.frameCount;
        StartCoroutine(PlayVideoCor());

        LeftArrow.SetActive(false);
        RightArrow.SetActive(false);
    }

    private void Update() {
        
        // 비디오 진행 슬라이더
        if (videoPlayer.isPlaying == true)
            m_sliderTimeScrub.value = (float)videoPlayer.frame;

        // 갈림길에 도달했을 때 플레이어의 방향전환에 따라 씬 변환
        if (m_sliderTimeScrub.maxValue * 0.99 <  m_sliderTimeScrub.value)
        {
            videoPlayer.Pause();
            LeftArrow.SetActive(true);
            RightArrow.SetActive(true);
            if (GestureStatus.text.Contains("Left"))
            {
                SceneManager.LoadScene(LeftScene);
            } else if (GestureStatus.text.Contains("Right"))
            {
                SceneManager.LoadScene(RightScene);
            }
        } else
        {
            // 현재 재스쳐를 인식하여 비디오 진행, 정지 결정
            if (GestureStatus.text.Contains("Run"))
            {
                videoPlayer.Play();
            }
            else
            {
                videoPlayer.Pause();
            }
        }
    }

    IEnumerator PlayVideoCor() {
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared) {
            yield return null;
        }

        m_rawImage.texture = videoPlayer.texture;
    }

    public void ChageValue() {
        videoPlayer.frame = (long)m_sliderTimeScrub.value;
    }
}