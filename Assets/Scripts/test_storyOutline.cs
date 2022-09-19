using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_storyOutline : MonoBehaviour {

    GameObject player;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");

        player.GetComponent<Player>().isActive = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
