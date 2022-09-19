using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test_dialog : MonoBehaviour {

    GameObject player;

    public Text text_name;
    public Text dialog;

    public string cha_name;
    public string[] dialog_text;

    public AudioSource dialog_sound_player;
    public AudioClip[] dialog_sound;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");

        player.GetComponent<Player>().isActive = false;

        StartCoroutine(dialog_start());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Chat(string narrator, string narration,int dialog_num)
    {
        int num = 0;
        text_name.text = narrator;
        dialog.text = "";

        string temp_dialog = "";

        dialog_sound_player.clip = dialog_sound[dialog_num];
        dialog_sound_player.Play();

        for (num = 0; num < narration.Length; num++)
        {
            temp_dialog += narration[num];

            dialog.text = temp_dialog;

            /* 클릭 했을때 대화 바로 진행되게
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                dialog.text = narration;
                Debug.Log("전부 띄움");
                break;
            }
            */

            //yield return new WaitForSeconds(0.1f);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator dialog_start()
    {
        int num = 0;
        for (num = 0; num < dialog_text.Length; num++) {
            yield return StartCoroutine(Chat(cha_name, dialog_text[num],num));

            while (true)
            {
                if (Input.GetKeyUp(KeyCode.Mouse0)) break;
                yield return new WaitForFixedUpdate();
            }
        }

        player.GetComponent<Player>().isActive = true;

        Debug.Log(player.GetComponent<Player>().isActive);

        gameObject.SetActive(false);
    }
}
