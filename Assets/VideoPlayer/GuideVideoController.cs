using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GuideVideoController : MonoBehaviour
{
    [SerializeField] RawImage m_rawImage = null;
    [SerializeField] VideoPlayer videoPlayer = null;
    [SerializeField] Slider m_sliderTimeScrub = null;

    // ��ȯ�� ��
    public string NextScene;

    void Start()
    {
        m_sliderTimeScrub.maxValue = videoPlayer.frameCount;
        StartCoroutine(PlayVideoCor());
    }

    private void Update()
    {
        // ���� ���� �����̴�
        if (videoPlayer.isPlaying == true)
        {
            m_sliderTimeScrub.value = (float)videoPlayer.frame;

        }

        // ���� ���� �� �� ��ȯ
        if (m_sliderTimeScrub.maxValue * 0.99 < m_sliderTimeScrub.value)
        {
            videoPlayer.Pause();
            LoadNextScene(NextScene);
        }
    }

    IEnumerator PlayVideoCor()
    {
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        m_rawImage.texture = videoPlayer.texture;
    }

    public void ChageValue()
    {
        videoPlayer.frame = (long)m_sliderTimeScrub.value;
    }

    // �� �ε�
    public void LoadNextScene(string sceneName)
    {
        // �񵿱������� Scene�� �ҷ����� ���� Coroutine�� ����Ѵ�.
        StartCoroutine(LoadMyAsyncScene(sceneName));
    }

    // �� �ε� ����
    IEnumerator LoadMyAsyncScene(string sceneName)
    {
        // AsyncOperation�� ���� Scene Load ������ �� �� �ִ�.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Scene�� �ҷ����� ���� �Ϸ�Ǹ�, AsyncOperation�� isDone ���°� �ȴ�.
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}