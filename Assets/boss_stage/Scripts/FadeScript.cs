using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class FadeScript : MonoBehaviour {

    public Image Panel;
    float time = 0f;
    float F_time = 1f; //페이드 시ㄱ나

    public void Fade () {
        StartCoroutine(FadeFlow());
	}

    public void Start()
    {
        StartCoroutine(FadeFlow());
    }
    IEnumerator FadeFlow()
    {
        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }
        time = 0f;
        yield return new WaitForSeconds(2f);
        while (alpha.a < 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
     
        Panel.gameObject.SetActive(false);
        yield return null;
    }
}
