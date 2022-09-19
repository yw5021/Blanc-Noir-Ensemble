using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgm : MonoBehaviour {

    // Use this for initialization
    void Start() {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update() {

    }

    public void bgm_change(AudioClip source) {
        gameObject.GetComponent<AudioSource>().clip = source;
    }

    public void bgm_destroy()
    {
        Destroy(gameObject);
    }
}
