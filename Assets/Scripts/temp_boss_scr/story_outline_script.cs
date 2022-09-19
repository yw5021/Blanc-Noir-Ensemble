using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class story_outline_script : MonoBehaviour {

    public VideoPlayer vp;

    public Image image;

    public GameObject dialog;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void videoPlay()
    {
        vp.Play();
    }

    public void image_set_active()
    {
        Invoke("destroy_img",1f);
    }

    void destroy_img()
    {
        Destroy(image.gameObject);
    }

    public void dialog_start()
    {
        Invoke("dialog_set_active", 4f);
    }

    void dialog_set_active()
    {
        dialog.SetActive(true);
    }
}
