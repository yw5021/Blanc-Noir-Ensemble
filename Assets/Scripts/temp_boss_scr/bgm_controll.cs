using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgm_controll : MonoBehaviour {

    GameObject go_bgm;

    public AudioClip clip;

	// Use this for initialization
	void Start () {
        go_bgm = GameObject.Find("bgm");

        go_bgm.GetComponent<bgm>().bgm_change(clip);
        go_bgm.GetComponent<AudioSource>().Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
