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

    // 전환할 씬
    public string NextScene;

    void Start()
    {
        m_sliderTimeScrub.maxValue = videoPlayer.frameCount;
        StartCoroutine(PlayVideoCor());
    }

    private void Update()
    {
        // 비디오 진행 슬라이더
        if (videoPlayer.isPlaying == true)
        {
            m_sliderTimeScrub.value = (float)videoPlayer.frame;

        }

        // 영상 종료 시 씬 변환
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

    // 씬 로드
    public void LoadNextScene(string sceneName)
    {
        // 비동기적으로 Scene을 불러오기 위해 Coroutine을 사용한다.
        StartCoroutine(LoadMyAsyncScene(sceneName));
    }

    // 씬 로드 헬프
    IEnumerator LoadMyAsyncScene(string sceneName)
    {
        // AsyncOperation을 통해 Scene Load 정도를 알 수 있다.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Scene을 불러오는 것이 완료되면, AsyncOperation은 isDone 상태가 된다.
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}