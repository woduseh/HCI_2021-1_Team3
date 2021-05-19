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

    static float calorieSpeed = 100f; // Į�θ� ǥ�� ���� �ֱ� (100f = 1/100�ʸ��� ����)

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

        // ���� ���� �����̴�
        if (videoPlayer.isPlaying == true)
        {
            m_sliderTimeScrub.value = (float)videoPlayer.frame;
            StartCoroutine("calorieBurn");

        } else if (videoPlayer.isPlaying == false)
        {
            StopCoroutine("calorieBurn");
        }

        // �����濡 �������� �� �÷��̾��� ������ȯ�� ���� �� ��ȯ
        if (m_sliderTimeScrub.maxValue * 0.99 <  m_sliderTimeScrub.value)
        {
            videoPlayer.Pause();

            // ���̵� �ɼ��� �����ְ�, ���̵� ������ �����ϸ� ���̵� ���� ����
            if (tourGuide == 1 && GuideScene != "")
            {
                LoadNextScene(GuideScene);
            }

            // �ش� �������� �̵� �����ϸ� ȭ��ǥ ǥ��
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
            // ���� �罺�ĸ� �ν��Ͽ� ���� ����, ���� ����
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

    // �� �ε�
    public void LoadNextScene(string sceneName)
    {
        // �����͸� ����
        SaveData();

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

    void LoadData()
    {
        calorie = PlayerPrefs.GetFloat("Cal");
        tourGuide = PlayerPrefs.GetInt("TourGuide");
        cal_indicator = PlayerPrefs.GetInt("Cal_Indicator");
    }

    void SaveData()
    {
        // �Ҹ� Į�θ� ���� 
        PlayerPrefs.SetFloat("Cal", calorie);
    }
}