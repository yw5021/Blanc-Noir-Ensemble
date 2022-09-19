using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class temp_text_alpha : MonoBehaviour {

    public Image fade;
    public Text text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Color color = text.color;

        color.a = fade.color.a;

        text.color = color;

        if(color.a >= 1)
        {
            SceneManager.LoadScene("TitleScene");

            GameObject go_bgm;
            go_bgm = GameObject.Find("bgm");
            go_bgm.GetComponent<bgm>().bgm_destroy();
        }
	}
}
