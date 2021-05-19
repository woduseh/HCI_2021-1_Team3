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
    public string CenterScene;
    public string GuideScene;
    public GameObject RightArrow;
    public GameObject LeftArrow;
    public GameObject CenterArrows;
    public GameObject CalorieIndicator;
    public UnityEngine.UI.Text GestureStatus;
    public UnityEngine.UI.Text CaloriesStatus;

    static float calorieSpeed = 100f; // 칼로리 표기 갱신 주기 (100f = 1/100초마다 갱신)

    float calorie;
    int tourGuide;
    int cal_indicator;

    void Start() {
        m_sliderTimeScrub.maxValue = videoPlayer.frameCount;
        StartCoroutine(PlayVideoCor());

        LeftArrow.SetActive(false);
        RightArrow.SetActive(false);
        CenterArrows.SetActive(false);

        videoPlayer.playbackSpeed = 1f;

        LoadData();

        if (cal_indicator == 1) CalorieIndicator.SetActive(true);

        StartCoroutine("calorieBurn");
    }

    private void Update() {
        CaloriesStatus.text = string.Format("{0:0.###}", calorie) + " Cal";

        // 비디오 진행 슬라이더
        if (videoPlayer.isPlaying == true)
        {
            m_sliderTimeScrub.value = (float)videoPlayer.frame;
            StartCoroutine("calorieBurn");

        } else if (videoPlayer.isPlaying == false)
        {
            StopCoroutine("calorieBurn");
        }

        // 갈림길에 도달했을 때 플레이어의 방향전환에 따라 씬 변환
        if (m_sliderTimeScrub.maxValue * 0.99 <  m_sliderTimeScrub.value)
        {
            videoPlayer.Pause();

            // 가이드 옵션이 켜져있고, 가이드 영상이 존재하면 가이드 영상 실행
            if (tourGuide == 1 && GuideScene != "")
            {
                LoadNextScene(GuideScene);
            }

            // 해당 방향으로 이동 가능하면 화살표 표시
            if (LeftScene != "") LeftArrow.SetActive(true);
            if (RightScene != "") RightArrow.SetActive(true);
            if (CenterScene != "") CenterArrows.SetActive(true);

            if (GestureStatus.text.Contains("Left"))
            {
                LoadNextScene(LeftScene);
            } else if (GestureStatus.text.Contains("Right"))
            {
                LoadNextScene(RightScene);
            } else if (GestureStatus.text.Contains("Run") || GestureStatus.text.Contains("walk"))
            {
                LoadNextScene(CenterScene);
            }
        } else
        {
            // 현재 재스쳐를 인식하여 비디오 진행, 정지 결정
            if (GestureStatus.text.Contains("Run"))
            {
                videoPlayer.playbackSpeed = 1.5f;
                videoPlayer.Play();
            }
            else if (GestureStatus.text.Contains("walk"))
            {
                videoPlayer.playbackSpeed = 1f;
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

    IEnumerator calorieBurn()
    {
        while (true)
        {
            float randNum = Random.Range(0.0625f, 0.1f);
            calorie = calorie + randNum / (videoPlayer.frameRate * calorieSpeed);

            yield return new WaitForSecondsRealtime(1.0f / calorieSpeed);
        }
    }

    // 씬 로드
    public void LoadNextScene(string sceneName)
    {
        // 데이터를 저장
        SaveData();

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

    void LoadData()
    {
        calorie = PlayerPrefs.GetFloat("Cal");
        tourGuide = PlayerPrefs.GetInt("TourGuide");
        cal_indicator = PlayerPrefs.GetInt("Cal_Indicator");
    }

    void SaveData()
    {
        // 소모 칼로리 저장 
        PlayerPrefs.SetFloat("Cal", calorie);
    }
}