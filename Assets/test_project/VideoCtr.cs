using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VideoCtr : MonoBehaviour
{
    public float animTime = 1f;         // Fade 애니메이션 재생 시간 (단위:초).  
    public VideoPlayer vp;
    private Image fadeImage;            // UGUI의 Image컴포넌트 참조 변수.  
    private Image fadeOutImage;
    private float start = 1f;           // Mathf.Lerp 메소드의 첫번째 값.  
    private float end = 0f;             // Mathf.Lerp 메소드의 두번째 값.  
    private float time = 0f;            // Mathf.Lerp 메소드의 시간 값.  

    private bool isPlaying = false;     // Fade 애니메이션의 중복 재생을 방지하기위해서 사용.  

    void Awake()
    {
        // Image 컴포넌트를 검색해서 참조 변수 값 설정.  
        fadeImage = GetComponent<Image>();
        fadeOutImage = GetComponent<Image>();
        vp = GetComponent<VideoPlayer>();
        vp.loopPointReached += OnMovieFinished;
    }
    void OnMovieFinished(VideoPlayer player)
    {
        Debug.Log("동영상끝");
        StartCoroutine("ChangeSceneTIme");
        StartCoroutine("PlayFadeOut");
        player.Stop();

    }
    // Fade 애니메이션을 시작시키는 메소드.  
    public void StartFadeAnim()
    {
        // 애니메이션이 재생중이면 중복 재생되지 않도록 리턴 처리.  
        if (isPlaying == true)
            return;

        // Fade 애니메이션 재생.  
        StartCoroutine("PlayFadeIn");
    }
    private void Start()
    {
        StartCoroutine("PlayFadeIn");

    }

    // Fade 애니메이션 메소드.  
    IEnumerator PlayFadeIn()
    {
        // 애니메이션 재생중.  
        isPlaying = true;

        // Image 컴포넌트의 색상 값 읽어오기.  
        Color color = fadeImage.color;
        time = 0f;
        color.a = Mathf.Lerp(start, end, time);

        while (color.a > 0f)
        {
            // 경과 시간 계산.  
            // 2초(animTime)동안 재생될 수 있도록 animTime으로 나누기.  
            time += Time.deltaTime / animTime;

            // 알파 값 계산.  
            color.a = Mathf.Lerp(start, end, time);
            // 계산한 알파 값 다시 설정.  
            fadeImage.color = color;
            vp.Play();
            yield return null;
        }
       while (vp.isPlaying)
        {
            yield return null;
        }
        // 애니메이션 재생 완료.  
        isPlaying = false;
    }
    IEnumerator PlayFadeOut()
    {
        // 애니메이션 재생중.  
        isPlaying = true;

        // Image 컴포넌트의 색상 값 읽어오기.  
        Color color = fadeOutImage.color;
        time = 0f;
        color.a = Mathf.Lerp(start, end, time);

        while (color.a < 1f)
        {
            // 경과 시간 계산.  
            // 2초(animTime)동안 재생될 수 있도록 animTime으로 나누기.  
            time += Time.deltaTime / animTime;

            // 알파 값 계산.  
            color.a = Mathf.Lerp(start, end, time);
            // 계산한 알파 값 다시 설정.  
            fadeImage.color = color;

            yield return null;
        }

        // 애니메이션 재생 완료.  
        isPlaying = false;
    }
    public IEnumerator ChangeSceneTIme()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("0-1");
    }
}
