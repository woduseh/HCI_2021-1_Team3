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

    // 전환할 씬
    public string RightScene;
    public string LeftScene;
    public string CenterScene;
    public string GuideScene;

    // 이동 가능 방향을 알리는 화살표
    public GameObject RightArrow;
    public GameObject LeftArrow;
    public GameObject CenterArrows;

    // 칼로리 표시기
    public GameObject CalorieIndicator;

    // 미니맵
    public GameObject Minimap;

    // 인식한 모션을 저장하는 UI
    public UnityEngine.UI.Text GestureStatus;

    // 소모한 칼로리를 표시하는 UI
    public UnityEngine.UI.Text CaloriesStatus;

    // 종료 경고 텍스트
    public GameObject ExitAlart;

    // 종료, 종료 취소 버튼
    public GameObject ExitButton;
    public GameObject CancelExitButton;

    static float calorieSpeed = 100f; // 칼로리 표기 갱신 주기 (100f = 1/100초마다 갱신)

    // Playerprefs에 저장된 소모 칼로리와 옵션
    float calorie;
    int tourGuide;
    int cal_indicator;
    int minimap;

    void Start() {
        LoadData();

        m_sliderTimeScrub.maxValue = videoPlayer.frameCount;
        StartCoroutine(PlayVideoCor());

        LeftArrow.SetActive(false);
        RightArrow.SetActive(false);
        CenterArrows.SetActive(false);
        ExitAlart.SetActive(false);
        ExitButton.SetActive(false);
        CancelExitButton.SetActive(false);

        videoPlayer.playbackSpeed = 1f;

        if (cal_indicator == 1) CalorieIndicator.SetActive(true); else CalorieIndicator.SetActive(false);
        if (minimap == 1) Minimap.SetActive(true); else Minimap.SetActive(false);

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
            // 종료 자세 (v 포즈 - 양팔 들기)를 취했을 때
            else if (GestureStatus.text.Contains("VPose"))
            {
                videoPlayer.Pause();
                ExitAlart.SetActive(true);
                ExitButton.SetActive(true);
                CancelExitButton.SetActive(true);
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

    // 칼로리 계산
    IEnumerator calorieBurn()
    {
        while (true)
        {
            float randNum = Random.Range(0.0625f, 0.1f);
            calorie = calorie + randNum / (videoPlayer.frameRate * calorieSpeed);

            yield return new WaitForSecondsRealtime(1.0f / calorieSpeed);
        }
    }

    // 투어 종료
    public void Quit()
    {
        Application.Quit();
    }

    // 투어 종료 취소
    public void CalcelExit()
    {
        ExitAlart.SetActive(false);
        ExitButton.SetActive(false);
        CancelExitButton.SetActive(false);
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
        minimap = PlayerPrefs.GetInt("Minimap");
    }

    void SaveData()
    {
        // 소모 칼로리 저장 
        PlayerPrefs.SetFloat("Cal", calorie);
    }
}