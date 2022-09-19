using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Optfalse : MonoBehaviour
{

    public Canvas test;
    bool isPause;


    void Update()
    {
        isPause = true;
    }

    public void Deative()
    {
 
        testst();
        Stopit();

    }

     void testst()
    {
        test.gameObject.SetActive(false);
    }
     void Stopit()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            /*일시정지 활성화*/
            if (isPause == true)
            {
                Time.timeScale = 0;
                isPause = false;
                return;
            }

            /*일시정지 비활성화*/
            if (isPause == false)
            {
                Time.timeScale = 1;
                isPause = true;
                return;
            }
        }

    }

}
