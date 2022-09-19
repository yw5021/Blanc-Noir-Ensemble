using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_destroy : MonoBehaviour {

    float delay = 1;

    float time = 0;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if (time > delay) Destroy(gameObject);
	}
}
