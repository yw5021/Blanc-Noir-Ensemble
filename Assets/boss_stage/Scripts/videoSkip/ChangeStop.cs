using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChage : MonoBehaviour
{
    public Image TesImage; //바뀌어질 이미지
    int a = 0;
    bool IsPause;

    void Start()
    {
        IsPause = true;
    }

    void ChangeImage()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            a = a + 1 ;
            Debug.Log(a = a + 1);
            if (a == 2)
            {
                Debug.Log("눌럿냐 ?");
                TesImage.gameObject.SetActive(true);
                
            } 
            else if(a == 4)
            {
                Debug.Log("2번째 눌러짐");
                TesImage.gameObject.SetActive(false);
                a = 0;
            }
        }

    }
    public void Reset()
    {
        TesImage.gameObject.SetActive(false);
    }
    public void Stopit()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            /*일시정지 활성화*/
            if (IsPause == true)
            {
                Time.timeScale = 0;
                IsPause = false;
                return;
            }

            /*일시정지 비활성화*/
            if (IsPause == false)
            {
                Time.timeScale = 1;
                IsPause = true;
                return;
            }
        }

    }

    void Update()
    {
        ChangeImage();
        Stopit();
    }
}
