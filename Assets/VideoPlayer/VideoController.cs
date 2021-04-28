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
        
        // ���� ���� �����̴�
        if (videoPlayer.isPlaying == true)
            m_sliderTimeScrub.value = (float)videoPlayer.frame;

        // �����濡 �������� �� �÷��̾��� ������ȯ�� ���� �� ��ȯ
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
            // ���� �罺�ĸ� �ν��Ͽ� ���� ����, ���� ����
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