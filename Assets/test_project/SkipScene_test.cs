using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipScene_test : MonoBehaviour
{
    public void skip_event()
    {
        FadeOutScript fade = GameObject.Find("Fade").GetComponent<FadeOutScript>();
        if (fade.isPlaying) return;
        fade.StartFadeAnim();
        Invoke("next_scene", fade.animTime);
    }

    void next_scene()
    {
        SceneManager.LoadScene("0-1");
    }

}
