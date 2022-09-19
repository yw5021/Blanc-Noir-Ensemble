using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stop : MonoBehaviour
{
    bool IsPause;

    // Use this for initialization
    void Start()
    {
        IsPause = true;

    }

    // Update is called once per frame
    void Update()
    {
        Stopit();
    }

    public void Stopit()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            /*일시정지 활성화*/
            if (IsPause == false)
            {
                Time.timeScale = 0;
                IsPause = true;
                return;
            }

            /*일시정지 비활성화*/
            if (IsPause == true)
            {
                Time.timeScale = 1;
                IsPause = false;               
                return;
            }
        }

    }

    void check()
    {
        

    }

}
